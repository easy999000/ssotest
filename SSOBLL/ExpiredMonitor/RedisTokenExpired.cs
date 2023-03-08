using Common;
using FreeSqlExtend;
using SSOBLL.DBModel;
using SSOBLL.Login;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.ExpiredMonitor
{
    /// <summary>
    /// token超时,控制
    /// </summary>
    public class RedisTokenExpired
    {
        /// <summary>
        /// 超时扫描,定时器
        /// </summary>
        private static System.Threading.Timer ScanExpiredLoginTokenTimer;
        /// <summary>
        /// 在线扫描定时器
        /// </summary>
        private static System.Threading.Timer ScanRenewalLoginTokenTimer;

        private string RedisConnStr;
        public RedisTokenExpired(string redisConnStr)
        {
            RedisConnStr = redisConnStr;
        }

        private Thread ListenerTh;

        /// <summary>
        /// 订阅redis消息
        /// </summary>
        /// <param name="token"></param>
        public void Subscribe()
        {
            //ListenerTh = new Thread(Listener);
            //ListenerTh.IsBackground = true;
            //ListenerTh.Start();
#if !DEBUG 
            RedisHelper.DBDefault.StringSet("SSO:WebThreadTestStart", DateTime.Now.ToString());
#endif
            try
            {
                Listener();
            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex, "Subscribe");
            }

            ScanExpiredLoginToken(null);
            ScanRenewalLoginToken(null);
        }

        /// <summary>
        /// 消息监听
        /// </summary>
        /// <param name="token"></param>
        private void Listener()
        {

            //__keyspace@0__:SSO:WebSiteAccountToken:COxDgE6YG02RxnswDl3I6G:expired
            //__keyspace@0__:SSO:WebSiteAccountToken:COxDgE6YG02RxnswDl3I6G:del
            // 暂时只监控这2种命令 
            LoggerHelper.LogTrace($"redis订阅启动");

            RedisHelper.Redis.DBConn.GetSubscriber().Subscribe(
                 $"__keyspace@*__:{Constant.RedisExpiredMonitorPrefix}*"
               //$"*"
               , (channel, message) =>
               {
                   try
                   {
                       var msgStr = channel + ":" + message;
                       LoggerHelper.LogTrace($"====收到消息====");
                       LoggerHelper.LogTrace(msgStr);

                       if (!RedisSubMsg.TryParse(msgStr, out var msg))
                       {
                           LoggerHelper.LogTrace(msgStr);
                           return;
                       }

                       var json = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                       LoggerHelper.LogTrace(json);

                       try
                       {
                           ProcessData(msg);
                       }
                       catch (Exception ex)
                       {
                           LoggerHelper.LogError(ex, ex.Message);
                       }

                   }
                   catch (Exception ex2)
                   {
                       LoggerHelper.LogError(ex2, "GetSubscriber().Subscribe");
                   }
               });

            //while (true)
            //{
            //queue.OnMessage(msg =>
            //{
            //    string log = "";
            //    log += $"msg.Message:{msg.Message},";
            //    log += $"msg.Message:{msg.Channel.ToString()},";
            //    LoggerHelper.LogTrace($"====收到消息====");
            //    LoggerHelper.LogTrace(msg);
            //});

            //} 
        }

        private void ProcessData(RedisSubMsg msg)
        {
            ///这个时候,redis的数据已经没有了
            if (msg.Command != "del" && msg.Command != "expired")
            {

                return;
            }

            var loginToken = LoginToken.GetLoginTokenByTokenFromDB(msg.Key);
            if (loginToken == null)
            {
                return;
            }

            LoginBLL bll = new LoginBLL();
            bll.ExitFromServer(loginToken);

        }


        /// <summary>
        /// 扫描超时的logintoken,  
        /// 系统重启的时候,超时的token消失会丢失,开机后,需要扫描一下.
        /// 并且定期几个小时,检查一下
        /// </summary>
        public void ScanExpiredLoginToken(object o)
        {
            string currentLoginToken = "";

            PageParam pageParam = new PageParam() { Count = 1, PageNumber = 0, PageSize = 100 };

            while (true)
            {
                var page = SqlHelper.Select<LoginToken>()
                     .Where(w => w.LoginToken.GreaterThan(currentLoginToken))
                     .OrderBy(o => o.LoginToken)
                     .ToPage(pageParam);
                if (page.Data.Count < 1)
                {
                    break;
                }
                currentLoginToken = page.Data.Last().LoginToken;
                pageParam.PageNumber++;

                LoginBLL bll = new LoginBLL();

                foreach (var item in page.Data)
                {
                    try
                    {
                        if (!LoginToken.ExistsLoginTokenRedis(item.LoginToken))
                        {
                            var loginToken = LoginToken.GetLoginTokenByTokenFromDB(item.LoginToken);
                            if (loginToken == null)
                            {
                                break;
                            }

                            bll.ExitFromServer(loginToken);
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.LogError(ex, ex.Message);

                    }
                }

            }


            if (ScanExpiredLoginTokenTimer == null)
            {
                ScanExpiredLoginTokenTimer = new Timer(ScanExpiredLoginToken, null, 1000 * 60 * 60, 1000 * 60 * 60);

            }

#if !DEBUG
            RedisHelper.DBDefault.StringSet("SSO:ScanExpiredLoginTokenTimer", DateTime.Now.ToString());
#endif


        }

        /// <summary>
        /// 扫描当前在线的账号信息.  并通知应用站点
        /// </summary>
        public void ScanRenewalLoginToken(object o)
        {

            ///考虑到多个站点并发,这块要做锁
            var rlock = RedisHelper.DBDefault.StringSet(Constant.RenewalLoginTokenLock, DateTime.Now.ToString()
                  , expiry: TimeSpan.FromMinutes(10)
                  , when: StackExchange.Redis.When.NotExists);
            if (rlock)
            {
                var webSiteList = WebSite.GetWebSiteInfoList();

                foreach (var webSite in webSiteList)
                {
                    string currentLoginToken = "";

                    PageParam pageParam = new PageParam() { Count = 1, PageNumber = 0, PageSize = 50 };
                    if (string.IsNullOrWhiteSpace(webSite.RenewalApi))
                    {
                        break;
                    }
                    int errorCount = 0;
                    while (true)
                    {
                        try
                        {

                            var page = WebSiteAccountToken.PageWebSiteAccountTokenBySecretKey(currentLoginToken, pageParam);
                            if (page.Data.Count < 1)
                            {
                                break;
                            }

                            currentLoginToken = page.Data.Last().LoginToken;
                            pageParam.PageNumber++;

                            WebSiteAccountToken.RenewalLoginTokenAsync(webSite.RenewalApi
                                , page.Data.Select(s => s.WebSiteAccountToken).ToList());


                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            if (errorCount > 2)
                            {
                                break;
                            }
                            LoggerHelper.LogError(ex, "ScanRenewalLoginToken");

                        }
                    }
                }
                ///取消锁
                RedisHelper.DBDefault.KeyDelete(Constant.RenewalLoginTokenLock);

            }


            if (ScanRenewalLoginTokenTimer == null)
            {
                ///20分钟
                ScanRenewalLoginTokenTimer = new Timer(ScanExpiredLoginToken, null, 1000 * 60 * 20, 1000 * 60 * 20);

            }

#if !DEBUG
            RedisHelper.DBDefault.StringSet("SSO:ScanRenewalLoginTokenTimer", DateTime.Now.ToString());
#endif


        }




    }
}

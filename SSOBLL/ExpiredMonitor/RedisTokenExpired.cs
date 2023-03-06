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
        private static System.Threading.Timer ScanExpiredLoginTokenTimer;
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
            Listener();
            ScanExpiredLoginToken();
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
            RedisHelper redis = new RedisHelper(RedisConnStr);
            //var queue = redis.DBConn.GetSubscriber().Subscribe($"__keyspace@*__:SSO:*");
            Trace.WriteLine($"redis订阅启动");
            redis.DBConn.GetSubscriber().Subscribe(
               $"__keyspace@*__:{Constant.RedisExpiredMonitorPrefix}*"
               // $"*"
               , (channel, message) =>
               {
                   var msgStr = channel + ":" + message;

                   if (!RedisSubMsg.TryParse(msgStr, out var msg))
                   {
                       Trace.WriteLine(msgStr);
                       return;
                   }

                   var json = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
                   Trace.WriteLine($"====收到消息====");
                   Trace.WriteLine(msgStr);
                   Trace.WriteLine(json);

                   try
                   {
                       ProcessData(msg);
                   }
                   catch (Exception ex)
                   {
                       Trace.TraceError(ex.StackTrace);
                   }
               });

            //while (true)
            //{
            //queue.OnMessage(msg =>
            //{
            //    string log = "";
            //    log += $"msg.Message:{msg.Message},";
            //    log += $"msg.Message:{msg.Channel.ToString()},";
            //    Trace.WriteLine($"====收到消息====");
            //    Trace.WriteLine(msg);
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
                pageParam.Count++;

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
                        Trace.TraceError(ex.Message);
                    }
                }

            }

            ScanExpiredLoginTokenTimer = new Timer(ScanExpiredLoginToken, null, 1000 * 60 * 60, 1000 * 60 * 60);
            


        }
    }
}

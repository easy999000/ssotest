using Common;
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

                   ProcessData(msg);
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

            LoginBLL bll=new LoginBLL();
            bll.ExitFromServer(loginToken); 

        }
    }
}

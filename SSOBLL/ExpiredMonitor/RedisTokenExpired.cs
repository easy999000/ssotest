using Common;
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
            RedisHelper redis = new RedisHelper(RedisConnStr);
            //var queue = redis.DBConn.GetSubscriber().Subscribe($"__keyspace@*__:SSO:*");
            Trace.WriteLine($"redis订阅启动");
            redis.DBConn.GetSubscriber().Subscribe(
               $"__keyspace@*__:{Constant.RedisExpiredMonitorPrefix}*"
               , (channel, message) =>
               {
                   Trace.WriteLine($"====收到消息====");
                   Trace.WriteLine(channel + "|" + message);
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
    }
}

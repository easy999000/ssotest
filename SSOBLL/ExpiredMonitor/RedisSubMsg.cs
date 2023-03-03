using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.ExpiredMonitor
{
    /// <summary>
    /// redis订阅消息
    /// </summary>
    public class RedisSubMsg
    {
        public RedisSubMsg() { }

        public string EventType { get; set; }

        public int DBIndex { get; set; }
        public string Command { get; set; }
        public string Key { get; set; }

        public static bool TryParse(string msgStr, out RedisSubMsg msg)
        {
            msg = null;
            try
            {

                //__keyspace@0__:SSO:WebSiteAccountToken:COxDgE6YG02RxnswDl3I6G:expired
                //__keyspace@0__:SSO:WebSiteAccountToken:COxDgE6YG02RxnswDl3I6G:del
                //__keyspace@0__:SSO:WebSiteAccountToken:COxDgE6YG02RxnswDl3I6G:expire
                //__keyevent@0__:expire:SSO:WebSiteAccountToken:COxDgE6YG02RxnswDl3I6G

                var index1 = msgStr.IndexOf('@');
                if (index1 < 3)
                {
                    return false;
                }

                ///取事件名字
                string eventType = msgStr.Substring(2, index1 - 2).ToLower();

                if (eventType != "keyspace" && eventType != "keyevent")
                {
                    return false;
                }

                msgStr = msgStr.Substring(index1 + 1);

                //取数据库
                var index2 = msgStr.IndexOf(':');
                if (index2 < 3)
                {
                    return false;
                }
                string DBIndexStr = msgStr.Substring(0, index2 - 2);
                if (!int.TryParse(DBIndexStr, out int DBIndex))
                {
                    return false;
                }
                msgStr = msgStr.Substring(index2 + 1);
                ////分割命令和键名
                string key = "";
                string command = "";

                int index3 = 0;
                if (eventType == "keyspace")
                {
                    index3 = msgStr.LastIndexOf(':');
                    key = msgStr.Substring(0, index3);
                    command = msgStr.Substring(index3 + 1).ToLower();
                }
                else
                {
                    index3 = msgStr.IndexOf(':');
                    command = msgStr.Substring(0, index3).ToLower();
                    key = msgStr.Substring(index3 + 1);
                }
                msg = new RedisSubMsg();
                msg.EventType = eventType;
                msg.DBIndex = DBIndex;
                msg.Command = command;
                msg.Key = key;


                return true;

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }
    }
}

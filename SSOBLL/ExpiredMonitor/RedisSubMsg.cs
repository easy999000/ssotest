using System;
using System.Collections.Generic;
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

        public string Message { get; set; }
        public int DBIndex { get; set; }
        public string Command { get; set; }
    }
}

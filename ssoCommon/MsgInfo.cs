using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoCommon
{
    public class MsgInfo
    {
        /// <summary>
        /// 0默认失败
        /// 1默认成功
        /// </summary>
        public int Code { get; set; }
        public string Msg { get; set; }
    }

    public class MsgInfo<T> : MsgInfo
    { 
        public T Data { get; set; }
    }

}

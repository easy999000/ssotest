using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// LoginTokenRedis前缀
        /// </summary>
        public const string LoginTokenRedisPrefix = "SSO:LoginToken";

        /// <summary>
        /// JumpTokenRedis前缀
        /// </summary>
        public const string JumpTokenRedisPrefix = "SSO:JumpToken";
        /// <summary>
        /// WebSiteTokenRedis前缀
        /// </summary>
        public const string WebSiteTokenRedisPrefix = "SSO:WebSiteToken";

        /// <summary>
        /// 秘钥控制器
        /// </summary>
        public const string DataProtectionRedisKey = "SSO:DataProtection-Keys";
    }
}

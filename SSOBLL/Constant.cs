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
        /// Redis超时监控前缀
        /// </summary>
        public const string RedisExpiredMonitorPrefix = "SSO:Expired:";

        /// <summary>
        /// LoginTokenRedis前缀
        /// </summary>
        public const string LoginTokenRedisPrefix = RedisExpiredMonitorPrefix + "LoginToken:";

        /// <summary>
        /// JumpTokenRedis前缀
        /// </summary>
        public const string JumpTokenRedisPrefix = "SSO:JumpToken:";
        /// <summary>
        /// WebSiteAccountToken前缀
        /// </summary>
        public const string WebSiteAccountTokenPrefix = "SSO:WebSiteAccountToken:";

        /// <summary>
        /// 秘钥控制器
        /// </summary>
        public const string DataProtectionRedisKey = "SSO:DataProtection-Keys";

        /// <summary>
        /// 授权站点列表数据缓存
        /// </summary>
        public const string WebSiteInfoListCatch = "SSO:WebSiteInfoListCatch";
        /// <summary>
        /// sso中心登入cookie名字
        /// </summary>
        public const string SSOCenterLoginCookieName = "SSO_SSOCenterLogin";
        /// <summary>
        /// 授权站点列表数据缓存
        /// </summary>
        public const string RoleAndWebSiteListCatch = "SSO:RoleAndWebSiteListCatch";
        /// <summary>
        /// 
        /// </summary>
        public const string RenewalLoginTokenLock = "SSO:RenewalLoginTokenLock";
    }
}

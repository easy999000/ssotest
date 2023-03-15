using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoClient.Models
{
    public class LoginAccount
    {
        /// <summary>
        /// 
        /// </summary>
        public LoginAccount() { }
        /// <summary>
        /// 登入的账号
        /// </summary>
        public string  Account { get; set; }

        /// <summary>
        /// 本次用户登入标识,每个用户,每次登入都不一样
        /// 和sso中心交互,都使用这个标识,不使用account
        /// </summary>
        public string WebSiteAccountToken { get; set; }

         
    }
}

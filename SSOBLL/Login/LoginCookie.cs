using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FreeSql.Internal.GlobalFilter;

namespace SSOBLL.Login
{
    public class LoginCookie
    {
        /// <summary>
        /// 可以同时登入不同的用户
        /// </summary>
        public List<(int RoleID, string LoginToken)> TokenList { get; set; } = new List<(int RoleID, string LoginToken)>();

        private IDataProtector _protector;
        /// <summary>
        /// 从输入cookie读取
        /// </summary>
        /// <param name="Cookies"></param>
        /// <param name="protectorProviderProvider"></param>
        /// <returns></returns>
        public static LoginCookie FromCookieValue(IRequestCookieCollection Cookies
            , IDataProtectionProvider protectorProviderProvider)
        {
            var protector = protectorProviderProvider.CreateProtector("SSOBLL.Login.DataProtector");
            var json = Cookies[Constant.SSOCenterLoginCookieName];
            if (json == null)
            {
                return new LoginCookie() { _protector= protector };
            }

//#if !DEBUG
            json=protector.Unprotect(json);
//#endif

            var cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginCookie>(json);
            cookie.RemoveLoginOutToken();

            cookie._protector = protector;

            return cookie;
        }
        /// <summary>
        /// 写入输出cookie
        /// </summary>
        /// <param name="Cookies"></param>
        public void ToCookieValue(IResponseCookies Cookies)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);

            //#if !DEBUG
            json = _protector.Protect(json);
            //#endif

            Cookies.Append(Constant.SSOCenterLoginCookieName, json);

        }
        /// <summary>
        /// 删除已经失效的登入token
        /// </summary>

        public void RemoveLoginOutToken()
        {
            for (int i = 0; i < TokenList.Count; i++)
            {
                var item = TokenList[i];
                var user = LoginToken.GetUserInfoByToken(item.LoginToken);
                if (user == null)
                {
                    TokenList.Remove(item);
                    i--;
                }

            }
        }

        /// <summary>
        /// 删除指定的loginCookie
        /// </summary>

        public void RemoveToken(LoginToken loginToken)
        {
            TokenList.Remove((loginToken.SSORoleID, loginToken.LoginToken));

        }

        /// <summary>
        /// 获取指定站点的登入用户
        /// 在多个登入用户中挑选第一个,具有当前站点登入权限的用户.
        /// </summary>
        /// <param name="webSiteID"></param>
        /// <returns></returns>
        public LoginToken GetCurrentLoginToken(int webSiteID)
        {
            foreach (var item in TokenList)
            {
                var user = LoginToken.GetUserInfoByToken(item.LoginToken);
                if (user != null && user.AllowWebSiteIDs.Contains(webSiteID))
                {
                    return user;
                }
            }

            return null;

        }

        /// <summary>
        /// 添加一个登入用户. 会把同一个角色的登入用户,覆盖掉
        /// </summary>
        /// <param name="loginToken"></param>

        public void AddLoginToken(LoginToken loginToken)
        {
            if (!TokenList.Any(a => a.LoginToken == loginToken.LoginToken && a.RoleID == loginToken.SSORoleID))
            {
                ///同样的角色,进行覆盖.
                TokenList.RemoveAll(r => r.RoleID == loginToken.SSORoleID);
                TokenList.Add((loginToken.SSORoleID, loginToken.LoginToken));
            }

        }

    }
}

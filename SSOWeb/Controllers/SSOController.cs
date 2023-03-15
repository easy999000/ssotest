using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SSOBLL;
using SSOBLL.ApiClient;
using SSOBLL.ApiClient.ApiModel;
using SSOBLL.Login;
using SSOWeb.Models;

namespace SSOWeb.Controllers
{
    public class SSOController : Controller
    {
        IDataProtectionProvider _protectorProviderProvider;
        public SSOController(IDataProtectionProvider protectorProviderProvider)
        {
            _protectorProviderProvider = protectorProviderProvider;
        }
        /// <summary>
        /// 登入或获取账号
        /// </summary>
        /// <param name="jumpUrl"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult Login(string jumpUrl = "", string account = "", string password = "")
        {
            var loginCookie = LoginCookie.FromCookieValue(this.Request.Cookies, _protectorProviderProvider);

            LoginBLL loginBLL = new LoginBLL();
            var check = loginBLL.CheckLoginStatus(jumpUrl, account, password
                , loginCookie);

            if (check.Code == 1)
            {
                UrlHelper jumpUrlHelper;
                UrlHelper.TryParse(jumpUrl, out jumpUrlHelper);

                jumpUrlHelper.AddQuery("jumptoken", check.Data.jumpToken.JumpToken);

                check.Data.loginCookie.ToCookieValue(this.Response.Cookies);
                return Redirect(jumpUrlHelper.GetUrl());

            }
            else if (check.Code == 2 || check.Code == 3)
            {
                ///这2种代码,不给做跳转
#if !DEBUG
                return Redirect($"https://www.hqbuy.com?sso={check.Code}");
#endif
            }
            LoginViewModel vm = new LoginViewModel() { Code = check.Code, Msg = check.Msg };
            var viewName = check.Data.ViewName;
            if (string.IsNullOrWhiteSpace(viewName))
            {
                viewName = "Login";
            }
            return View(viewName, vm);
        }

        /// <summary>
        /// 返回账号状态
        /// </summary>
        /// <param name="jumpUrl"></param>
        /// <returns></returns>
        public IActionResult CheckLoginStatus(string jumpUrl)
        {
            var loginCookie = LoginCookie.FromCookieValue(this.Request.Cookies, _protectorProviderProvider);

            LoginBLL loginBLL = new LoginBLL();
            var check = loginBLL.CheckLoginStatus(jumpUrl, "", ""
                , loginCookie);

            UrlHelper jumpUrlHelper;
            UrlHelper.TryParse(jumpUrl, out jumpUrlHelper);

            if (check.Code == 1)
            {
                jumpUrlHelper.AddQuery("jumptoken", check.Data.jumpToken.JumpToken);
                check.Data.loginCookie.ToCookieValue(this.Response.Cookies);
                return Redirect(jumpUrlHelper.GetUrl());
            }
            else if (check.Code == 2 || check.Code == 3)
            {
                ///这2种代码,不给做跳转#if !DEBUG
#if !DEBUG
                return Redirect($"https://www.hqbuy.com?sso={check.Code}");
#endif
            }

            ///未登录返回空状态
            jumpUrlHelper.AddQuery("jumptoken", "null");
            return Redirect(jumpUrlHelper.GetUrl());


        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="jumpUrl"></param>
        /// <returns></returns>
        public IActionResult Logout(string jumpUrl = "")
        {
            if (string.IsNullOrWhiteSpace(jumpUrl))
            {
                return Redirect($"https://www.hqbuy.com?sso={0}");
            }
            var loginCookie = LoginCookie.FromCookieValue(this.Request.Cookies, _protectorProviderProvider);

            UrlHelper jumpUrlHelper;
            UrlHelper.TryParse(jumpUrl, out jumpUrlHelper);

            LoginBLL loginBLL = new LoginBLL();
            var check = loginBLL.Logout(jumpUrl
                , loginCookie);


            if (check.Code == 1)
            {
                check.Data.ToCookieValue(this.Response.Cookies);
                return Redirect(jumpUrlHelper.GetUrl());
            }
            else if (check.Code == 2 || check.Code == 3)
            {
                ///这2种代码,不给做跳转#if !DEBUG
#if !DEBUG
                return Redirect($"https://www.hqbuy.com?sso={check.Code}");
#endif
            }

            jumpUrlHelper.AddQuery("sso", check.Code.ToString());
            jumpUrlHelper.AddQuery("msg", check.Msg);

            //带回错误代码

            return Redirect(jumpUrlHelper.GetUrl());

        }

        [Authorize(Policy = "ApiJwtPllicy")]
        public ApiMsg<LoginAccount> CheckJumpToken([FromBody] CheckJumpTokenParam param = null)
        {
            LoginBLL loginBLL = new LoginBLL();
            return loginBLL.CheckJumpToken(param.JumpToken, param.WebSiteMark);

        }
    }
}

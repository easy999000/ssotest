using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ssoClient.Common;
using ssoClient.Models;
using ssoCommon;
using ssoCommon.Api;

namespace ssoClient.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 模拟会员中心
        /// </summary>
        /// <returns></returns>
        public IActionResult Main()
        {
            ////这里面部分功能,应该是放在权限中间件里面的,这里为了演示,简单实现
            ///

            string centerUrl = $"{ConfigOption.DefaultConfig.CenterDomain}/sso/login?jumpurl={Uri.EscapeDataString($"{ConfigOption.DefaultConfig.CurrentDomain}/client/VerifyLogin?mark=main")}";

            var loginID = this.Request.Cookies["logintoken"];

            ///模拟数据库
            var user = AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).GetData<UserInfo>(loginID);

            if (user == null
                || string.IsNullOrWhiteSpace(user.Account))
            {
                ///未登录
                return this.Redirect(centerUrl);
            }

            ViewBag.User = user;
            return View();
        }
        /// <summary>
        /// 接收sso中心的登入令牌,并验证.
        /// </summary>
        /// <param name="jumptoken"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IActionResult VerifyLogin(string jumptoken, string mark = "")
        {

            var check = APIClient.CheckJumpTokenAsync(new CheckJumpTokenParam
            {
                JumpToken = jumptoken
              ,
                WebSiteMark = WebSiteConfig.Config.WebSiteMark
            }).Result;

            if (check == null || check.Code != 1)
            {
                return Content("授权无效");
            }

            this.Response.Cookies.Append("logintoken", check.Data.WebSiteAccountToken);

            ///存储登陆信息 
            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).SetData(check.Data.WebSiteAccountToken
                , new UserInfo { LoginMark= check.Data.WebSiteAccountToken
                , Account= check.Data.Account
                });

            if (string.IsNullOrWhiteSpace(mark))
            {
                mark = "Main";
            }


            return RedirectToAction(mark);

        } 

        /// <summary>
        /// 退出登入
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Policy = "ApiJwtPllicy")]
        public ApiMsg SSOLogout([FromBody]WebsiteLogoutParam param = null)
        {
            if (param == null)
            {
                return new ApiMsg { Code = 1, Data = this.User.Claims.Count() };
            }
            return new ApiMsg { Code = 1, Data = this.User.Claims.Count() };
        }

        /// <summary>
        /// 账号登入状态续期
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Policy = "ApiJwtPllicy")]
        public ApiMsg SSORenewalAccount([FromBody] RenewalAccountParam param = null)
        {
            if (param == null)
            {
                return new ApiMsg { Code = 1, Data = this.User.Claims.Count() };
            }
            return new ApiMsg { Code = 1, Data = this.User.Claims.Count() };
        }



    }
}

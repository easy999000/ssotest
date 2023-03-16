using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ssoClient.Common;
using ssoClient.Models;
 

namespace ssoClient.Controllers
{
    public class ClientController : Controller
    { 

        /// <summary>
        /// 模拟会员中心
        /// 如果当前未登录,跳转到登入页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Main()
        {
            ////这里面部分功能,应该是放在权限中间件里面的,这里为了演示,简单实现
            ///

            string centerUrl = $"{ConfigOption.DefaultConfig.CenterDomain}/sso/login?jumpurl={Uri.EscapeDataString($"{ConfigOption.DefaultConfig.CurrentDomain}/client/VerifyLogin?mark=main")}";

            var loginID = this.Request.Cookies["WebSiteAccountToken"];

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
        /// 验证通过后,进行登入操作.并跳转到指定页面
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

            ///正式系统要做加密,不要把 WebSiteAccountToken暴露出去
            this.Response.Cookies.Append("WebSiteAccountToken", check.Data.WebSiteAccountToken);

            ///存储登陆信息 
            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).SetData(check.Data.WebSiteAccountToken
                , new UserInfo
                {
                    LoginMark = check.Data.WebSiteAccountToken
                ,
                    Account = check.Data.Account
                });

            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).SetData<long>("RefreshTime", DateTime.Now.ToFileTime());

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
        public ApiMsg SSOLogout([FromBody] WebsiteLogoutParam param = null)
        {
            if (param == null)
            {
                return ApiMsg.ReturnError("token无效");
            }

            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).DelData(param.WebSiteAccountToken);

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
                return new ApiMsg { Code = 1, Data =0 };
            }

            foreach (var item in param.WebSiteAccountTokenList)
            {
                AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).Renewal(item);
            } 

            return new ApiMsg { Code = 1, Data = this.User.Claims.Count() };
        }

        

    }
}

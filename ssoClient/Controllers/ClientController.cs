﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult main()
        {
            string centerUrl = $"{ConfigOption.DefaultConfig.CenterDomain}/sso/login?backurl={Uri.EscapeDataString(ConfigOption.DefaultConfig.CurrentDomain)}/client/verifyLogin";

            var loginID = this.Request.Cookies["logintoken"];

            var user = AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).GetData<UserInfo>(loginID);

            if (user == null
                || string.IsNullOrWhiteSpace(user.Name))
            {
                this.HttpContext.Session.SetString("actionName", "main");
                return this.Redirect(centerUrl);

            }

            ViewBag.User = user;
            return View();
        }

        public IActionResult verifyLogin(string ssotoken)
        {
            CenterApi api = new CenterApi(ConfigOption.DefaultConfig.CenterDomain);

            var verify = api.Client.VerifyToken(ssotoken).Result;
            if (verify == null || verify.Code == 0)
            {
                return Content("授权无效");
            }

            this.Response.Cookies.Append("logintoken", ssotoken);
            ///存储登陆信息 
            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).SetData(ssotoken, verify.Data);


            return RedirectToAction(this.HttpContext.Session.GetString("actionName"));

        }

        public MsgInfo<string> logout(string loginID)
        {
            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).DelData(loginID);
            return new MsgInfo<string> { Code = 1 };

        }


    }
}
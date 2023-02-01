using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ssoCommon;
using ssoCommon.Api;

namespace ssoCenter.Controllers
{
    public class SSOController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login(string name, string password, string backurl = "")
        {
            this.ViewData["msg"] = string.Empty;
            var loginID = this.Request.Cookies["logintoken"];
            if (!string.IsNullOrWhiteSpace(loginID))
            {

                var tokenValue = AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).GetData<UserInfo>(loginID);

                if (tokenValue != null
                    && !string.IsNullOrWhiteSpace(tokenValue.LoginMark))
                {
                    var b1 = UrlHelper.TryParse(backurl, out UrlHelper urlHelper);

                    if (!b1)
                    {
                        return this.Redirect(this.Request.Scheme + "://" + this.Request.Host.ToString());
                    }
                    urlHelper.AddQuery("ssotoken", loginID);
                    return this.Redirect(urlHelper.GetUrl());

                }
            }
            ///有无账号信息
            if (string.IsNullOrWhiteSpace(name))
            {
                return View();
            }
            ///验证秘密
            var pass = AnalogData.GetAnalogData(AnalogDataEnum.Password).GetData<string>(name);
            if (pass != password)
            {
                this.ViewData["msg"] = "密码或账号错误";
                return View();
            }
            ///登陆成功,生成令牌
            ///

            var rand = new Random();

            var baseKey = $"{name}{DateTime.Now.ToFileTime()}";

            ///严谨来说,用户的cookie令牌,授权跳转登录令牌,本地存储登陆信息的key,应该是各不相同.
            ///并且授权跳转令牌时间不宜过长,而且要防止撞库.
            ///这里演示不做太严谨处理.

            loginID = $"loginKey{baseKey}";
            var loginMark = $"{baseKey}|{rand.Next(1000, 9999)}";

            UserInfo loginUser = new UserInfo();
            loginUser.LoginTime = DateTime.Now;
            loginUser.Name = name;
            loginUser.LoginMark = loginMark;

            ///存储登陆信息 
            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).SetData(loginID, loginUser);


            //设置用户cookie
            string cookieDomain = "";
            if (this.Request.Host.Host.Contains('.'))
            {
                var hostList = this.Request.Host.Host.Split(".");
                cookieDomain = $"{hostList[hostList.Length - 2]}.{hostList[hostList.Length - 1]}";

            }

            this.Response.Cookies.Append("logintoken", loginID, new CookieOptions { Domain = cookieDomain });


            var b2 = UrlHelper.TryParse(backurl, out UrlHelper urlHelper2);

            if (!b2)
            {
                return this.Redirect(this.Request.Scheme + "://" + this.Request.Host.ToString());
            }
            urlHelper2.AddQuery("ssotoken", loginID);
            return this.Redirect(urlHelper2.GetUrl());


        }

        public MsgInfo<UserInfo> VerifyToken(string loginID)
        {

            var tokenValue = AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).GetData<UserInfo>(loginID);

            if (tokenValue != null
                && !string.IsNullOrWhiteSpace(tokenValue.LoginMark))
            {
                return new MsgInfo<UserInfo> { Code = 1, Msg = "", Data = tokenValue };


            }
            else
            {
                return new MsgInfo<UserInfo> { Code = 0, Msg = "无效" };
            }
        }


        public IActionResult Logout(string backurl = "")
        {
            var loginID = this.Request.Cookies["logintoken"];

            AnalogData.GetAnalogData(AnalogDataEnum.LoginUser).DelData(loginID);

            foreach (var item in ConfigOption.DefaultConfig.ClientDomain)
            {
                try
                {

                    var api = new ClientApi(item);
                    var msg = api.Client.Logout(loginID).Result;

                }
                catch (Exception er)
                {
                    Console.WriteLine(er.Message);
                }

            }

            ///跳转页面
            /// 


            var b2 = UrlHelper.TryParse(backurl, out UrlHelper urlHelper2);

            if (!b2)
            {
                return this.Redirect(this.Request.Scheme + "://" + this.Request.Host.ToString());
            }
            urlHelper2.AddQuery("ssotoken", loginID);
            return this.Redirect(urlHelper2.GetUrl());



        }
    }
}

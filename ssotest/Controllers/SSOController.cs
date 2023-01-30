using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;


namespace ssotest.Controllers
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
            if (!string.IsNullOrWhiteSpace(this.Request.Cookies["logintoken"]))
            {

                var tokenValue = AnalogData.GetAnalogData("cookie").GetData(this.Request.Cookies["logintoken"]);

                if (tokenValue == this.Request.Cookies["logintoken"] + "|aaa")
                {
                    var b1 = UrlHelper.TryParse(backurl, out UrlHelper urlHelper);

                    if (!b1)
                    {
                        return this.Redirect(this.Request.Scheme + "://" + this.Request.Host.ToString());
                    }
                    urlHelper.AddQuery("ssokey", this.Request.Cookies["logintoken"]);
                    return this.Redirect(urlHelper.GetUrl());

                }
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return View();
            }

            var pass = AnalogData.GetAnalogData("password").GetData(name);
            if (pass != password)
            {
                this.ViewData["msg"] = "密码或账号错误";
                return View();
            }
            var key = name + $"{DateTime.Now.ToFileTime()}";
            var value = key + "|aaa";
            AnalogData.GetAnalogData("cookie").SetData(key, value);

            string cookieDomain = "";
            if (this.Request.Host.Host.Contains('.'))
            {
                var hostList = this.Request.Host.Host.Split(".");
                cookieDomain = $"{hostList[hostList.Length - 2]}.{hostList[hostList.Length - 1]}";

            }

            this.Response.Cookies.Append("logintoken", key,new CookieOptions { Domain= cookieDomain });


            var b2 = UrlHelper.TryParse(backurl, out UrlHelper urlHelper2);

            if (!b2)
            {
                return this.Redirect(this.Request.Scheme + "://" + this.Request.Host.ToString());
            }
            urlHelper2.AddQuery("ssokey", key);
            return this.Redirect(urlHelper2.GetUrl());


        }


    }
}

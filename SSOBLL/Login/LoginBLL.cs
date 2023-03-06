using Common;
using SSOBLL.ApiClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class LoginBLL
    {
        /// <summary>
        /// 1.成功获取用户状态
        /// 2.跳转url无效
        /// 3.跳转站点未授权
        /// 4.用户未登录
        /// 5.账号密码错误
        /// 6.账号角色未设置
        /// 7.账号未授权
        /// </summary>
        /// <param name="jumpUrl"></param>
        /// <param name="account"></param>
        /// <param name="pass"></param>
        /// <param name="WebSiteID"></param>
        /// <returns></returns>
        public ApiMsg<(JumpToken jumpToken, LoginCookie loginCookie, string ViewName)> CheckLoginStatus(string jumpUrl
            , string account, string pass
            , LoginCookie loginCookie)
        {
            var res = ApiMsg<(JumpToken jumpToken, LoginCookie loginCookie, string ViewName)>
                .ReturnError("未知错误");

            UrlHelper jumpUrlHelper;
            ///检测回跳地址,是否合规
            if (string.IsNullOrWhiteSpace(jumpUrl) ||
                !UrlHelper.TryParse(jumpUrl, out jumpUrlHelper))
            {
                return res.SetError(2, "jumpUrl无效");
            }
            ///获取准备登入的站点, 判断跳转站点是否是授权站点

            var site = WebSite.GetWebSiteInfoByHost(jumpUrlHelper.uriBuilder.Host);
            if (site == null)
            {
                return res.SetError(3, "站点未授权");
            }
            res.Data = (null, null, site.ViewName);
            ///判断用户是否已经登入
            /// 

            var loginToken2 = loginCookie.GetCurrentLoginToken(site.ID);
            if (loginToken2 != null)
            {
                var webAccount = loginToken2.WebSiteAccountList.FirstOrDefault(f => f.WebSiteSecretKey == site.WebSiteSecretKey);

                if (webAccount == null)
                {
                    ///用户站点令牌信息
                    var webSiteAccountToken2 = WebSiteAccountToken.MakeWebSiteAccountToken(loginToken2.LoginToken, site.WebSiteSecretKey);
                    webAccount = new WebsiteAccountDTO
                    {
                        WebSiteAccountToken = webSiteAccountToken2.WebSiteAccountToken,
                        WebSiteID = site.ID,
                        WebSiteSecretKey = site.WebSiteSecretKey
                    };
                    loginToken2.WebSiteAccountList.Add(webAccount);
                    loginToken2.SaveLoginTokenToRedis();
                }

                ///生成跳转令牌
                JumpToken jumpToken2 = JumpToken.MakeJumpToken(webAccount.WebSiteAccountToken, webAccount.WebSiteSecretKey);


                LoginToken.DelayedExpire(loginToken2.LoginToken);

                // var webAccountTokenList = WebSiteAccountToken.ListWebSiteAccountToken(loginToken2.LoginToken);

                WebSiteAccountToken.DelayedExpire(loginToken2.WebSiteAccountList.Select(s => s.WebSiteAccountToken).ToArray());

                ///返回用户信息
                return res.SetSuccess((jumpToken2, loginCookie, site.ViewName));
            }

            ///当前未登录,判断是否有输入,账号密码,

            if (string.IsNullOrWhiteSpace(account))
            {
                ///无账号不做登入
                return res.SetError(4, "用户未登录");
            }

            ///验证账号密码
            ApiClient.AccountCenterApi accountClient = new ApiClient.AccountCenterApi();
            var loginMsg = accountClient.Login(account, pass);
            if (loginMsg.Code != 1)
            {
                return res.SetError(5, loginMsg.Msg);
            }

            ///判断用户角色和站点的权限
            ///获取用户角色
            Role roleBll = new Role();
            var role = Role.GetRoleByID(loginMsg.Data.RoleID);
            if (role == null)
            {
                return res.SetError(6, "用户角色未设置");
            }

            if (!role.CanWebSite(site.ID))
            {
                //判断当前用户角色,是否包含当前用户站点

                return res.SetError(7, "账号未授权");
            }

            ///生成账号在线登入令牌信息
            LoginToken loginToken3 = LoginToken.MakeLoginToken(account, role.ID);

            ///生成站点令牌信息
            WebSiteAccountToken webSiteAccountToken3 = WebSiteAccountToken.MakeWebSiteAccountToken(loginToken3.LoginToken, site.WebSiteSecretKey);

            loginToken3.WebSiteAccountList.Add(new WebsiteAccountDTO
            {
                WebSiteAccountToken = webSiteAccountToken3.WebSiteAccountToken,
                WebSiteID = site.ID,
                WebSiteSecretKey = site.WebSiteSecretKey
            });

            loginToken3.SaveLoginTokenToRedis();
            ///生成跳转令牌
            JumpToken jumpToken3 = JumpToken.MakeJumpToken(webSiteAccountToken3.WebSiteAccountToken, webSiteAccountToken3.WebSiteSecretKey);

            loginCookie.AddLoginToken(loginToken3);

            return res.SetSuccess((jumpToken3, loginCookie, site.ViewName));

        }


        /// <summary>
        /// 1.成功获取用户状态
        /// 2.跳转url无效
        /// 3.跳转站点未授权
        /// 4.用户未登录
        /// 5.账号密码错误
        /// </summary>
        /// <param name="jumpUrl"></param>
        /// <param name="account"></param>
        /// <param name="pass"></param>
        /// <param name="WebSiteID"></param>
        /// <returns></returns>
        public ApiMsg<LoginCookie> Logut(string jumpUrl
            , LoginCookie loginCookie)
        {
            var res = ApiMsg<LoginCookie>.ReturnError("未知错误");

            UrlHelper jumpUrlHelper;
            ///检测回跳地址,是否合规
            if (string.IsNullOrWhiteSpace(jumpUrl) ||
                !UrlHelper.TryParse(jumpUrl, out jumpUrlHelper))
            {
                return res.SetError(2, "jumpUrl无效");
            }
            ///获取准备登入的站点, 判断跳转站点是否是授权站点

            var site = WebSite.GetWebSiteInfoByHost(jumpUrlHelper.uriBuilder.Host);
            if (site == null)
            {
                return res.SetError(3, "站点未授权");
            }
            ///判断用户是否已经登入
            /// 

            var loginToken2 = loginCookie.GetCurrentLoginToken(site.ID);
            if (loginToken2 != null)
            {
                ///已登入
                loginCookie.RemoveToken(loginToken2);

                ExitFromServer(loginToken2);

                ///返回用户信息
                return res.SetSuccess(loginCookie);
            }

            return res.SetSuccess(loginCookie);
        }

        /// <summary>
        /// 清除登入状态.
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        internal bool ExitFromServer(LoginToken loginToken)
        {
            ///会出现并发,先后发等情况, 要加处理条件.
            ///退出中心状态
            var isDel = LoginToken.DelLoginToken(loginToken.LoginToken);

            if (!isDel)
            {
                return isDel;
            }

            // var webSiteAccountTokenList = WebSiteAccountToken.ListWebSiteAccountToken(loginToken);

            WebSiteAccountToken.DelWebsiteAccountToken(
                loginToken.WebSiteAccountList.Select(s => s.WebSiteAccountToken).ToArray());

            //if (webSiteAccountTokenList.Count < 1)
            //{
            //    return isDel;
            //}

            List<Task> tasks = new List<Task>();
            ///退出登入站点
            foreach (var item in loginToken.WebSiteAccountList)
            {
                var website = WebSite.GetWebSiteInfoByID(item.WebSiteID);

                if (website == null || string.IsNullOrWhiteSpace(website.LogoutApi))
                {
                    break;
                }
                try
                {
                    var t1 = WebSiteClient.LogoutAccountAsync(website.LogoutApi, item.WebSiteAccountToken)
                        .ContinueWith((tres) =>
                        {
                            if (tres.Status == TaskStatus.Faulted)
                            { 
                                if (tres.Exception != null)
                                {
                                    Trace.TraceError(tres.Exception.Message);
                                }
                            }
                             
                        });

                    tasks.Add(t1);

                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                }
            }

            if (tasks.Count > 0)
            {
                Task.WhenAll(tasks.ToArray()).Wait();

                //foreach (var item in tasks)
                //{
                //    if (item.Exception != null)
                //    {
                //        Trace.TraceError(item.Exception.Message);
                //    }
                //}
            }


            return isDel;
        }

    }
}

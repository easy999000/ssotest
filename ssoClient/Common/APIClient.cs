
using Microsoft.AspNetCore.Mvc;
using ssoClient.Common.JWT;
using ssoClient.Models;
using ssoCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ssoClient.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class APIClient
    {
        public static APIClient StaticClient = new APIClient();

        HttpClient client;
        public APIClient()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(60) // Recreate every 15 minutes
            };
            client = new HttpClient(handler);
        }

        public async Task<T> ApiPostAsync<T>(string url, object data, string JwtToken = null) where T : class
        {

            var content = JsonContent.Create(data);
            //   var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            var response3 = await client.SendAsync(request);

            var json = await response3.Content.ReadAsStringAsync();


            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

            return res;
        }



        /// <summary>
        /// 检查 jumptoken,如果成功,返回登入用户信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webSiteAccountToken"></param>
        /// <returns></returns>
        public static async Task<ApiMsg<LoginAccount>> CheckJumpTokenAsync(CheckJumpTokenParam param)
        {
            string url = ConfigOption.DefaultConfig.CenterDomain + @"/sso/CheckJumpToken";

            var jwtToken = JwtHelper.CreateToken(WebSiteConfig.Config.WebSiteMark, WebSiteConfig.Config.JwtSecret);


            return await StaticClient.ApiPostAsync<ApiMsg<LoginAccount>>(url, param, jwtToken);
        }

        /// <summary>
        /// 续期账号状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns> 
        public static async Task<ApiMsg> RenewaWebSiteAccount(RenewaWebSiteAccountParam param)
        {
            string url = ConfigOption.DefaultConfig.CenterDomain + @"/sso/RenewaWebSiteAccount";

            var jwtToken = JwtHelper.CreateToken(WebSiteConfig.Config.WebSiteMark, WebSiteConfig.Config.JwtSecret);

            return await StaticClient.ApiPostAsync<ApiMsg>(url, param, jwtToken);
        }

    }
}

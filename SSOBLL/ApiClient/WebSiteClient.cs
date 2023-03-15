using SSOBLL.ApiClient.ApiModel;
using SSOBLL.DBModel;
using SSOBLL.JWT;
using SSOBLL.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.ApiClient
{
    /// <summary>
    /// 
    /// </summary>
    public class WebSiteClient
    {
        public static WebSiteClient StaticClient = new WebSiteClient();

        HttpClient client;
        public WebSiteClient()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(60) // Recreate every 15 minutes
            };
            client = new HttpClient(handler);
        }

        public async Task<T> ApiPostAsync<T>(string url, object data, string JwtToken = null) where T : class
        {

            //var response = await client.PostAsJsonAsync(url, data);

            //var res = await response.Content.ReadFromJsonAsync<T>();


            //if (!string.IsNullOrWhiteSpace(JwtToken))
            //{
            //    content.Headers.Add("Authorization", $"Bearer {JwtToken}");

            //}
            //var response2 = await client.PostAsync(url
            //    , content
            //     );

            //var json = await response2.Content.ReadAsStringAsync();
            var content = JsonContent.Create(data);
           // var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = content;

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JwtToken);

            var response3 = await client.SendAsync(request);

            var json = await response3.Content.ReadAsStringAsync();


            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

            return res;
        }
        /// <summary>
        /// 退出账号
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webSiteAccountToken"></param>
        /// <returns></returns>
        public static async Task<ApiMsg> LogoutAccountAsync(string url, string webSiteAccountToken, WebSiteInfo webSite)
        {
            var secret = WebSite.GetWebSiteSecret_Catch(webSite.WebSiteMark);

            var jwtToken = JwtHelper.CreateToken(webSite.WebSiteMark, secret.Key1);

            WebsiteLogoutParam param = new WebsiteLogoutParam() { WebSiteAccountToken = webSiteAccountToken };

            return await StaticClient.ApiPostAsync<ApiMsg>(url, param, jwtToken);
        }
        /// <summary>
        /// 续期账号在线状态
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webSiteAccountToken"></param>
        /// <returns></returns>
        public static async Task<ApiMsg> RenewalAccountAsync(string url, List<string> webSiteAccountTokenList, WebSiteInfo webSite)
        {
            var secret = WebSite.GetWebSiteSecret_Catch(webSite.WebSiteMark);

            var jwtToken = JwtHelper.CreateToken(webSite.WebSiteMark, secret.Key1);

            RenewalAccountParam param = new RenewalAccountParam() { WebSiteAccountTokenList = webSiteAccountTokenList };

            return await StaticClient.ApiPostAsync<ApiMsg>(url, param, jwtToken);

        }

    }
}

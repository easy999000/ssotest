using SSOBLL.ApiClient.ApiModel;
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

        public async Task<T> ApiPostAsync<T>(string url, object data) where T : class
        {
            var response = await client.PostAsJsonAsync(url, data);

            var res = await response.Content.ReadFromJsonAsync<T>();

            return res;
        }
        /// <summary>
        /// 退出账号
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webSiteAccountToken"></param>
        /// <returns></returns>
        public static async Task<ApiMsg> LogoutAccountAsync(string url, string webSiteAccountToken)
        {
            WebsiteLogoutParam param = new WebsiteLogoutParam() { WebSiteAccountToken = webSiteAccountToken };

            return await StaticClient.ApiPostAsync<ApiMsg>(url, param);
        }
        /// <summary>
        /// 续期账号在线状态
        /// </summary>
        /// <param name="url"></param>
        /// <param name="webSiteAccountToken"></param>
        /// <returns></returns>
        public static async Task<ApiMsg> RenewalAccountAsync(string url, List<string> webSiteAccountTokenList)
        {
            RenewalAccountParam param = new RenewalAccountParam() {  WebSiteAccountTokenList = webSiteAccountTokenList };

            return await StaticClient.ApiPostAsync<ApiMsg>(url, param);

        }

    }
}

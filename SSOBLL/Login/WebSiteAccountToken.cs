using Common;
using FreeSql.DataAnnotations;
using FreeSqlExtend;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using SSOBLL.ApiClient;
using SSOBLL.DBModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    /// <summary>
    /// 站点用户令牌对照表
    /// </summary>
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true, Name = nameof(WebSiteAccountTokenInfo))]
    public class WebSiteAccountToken : DBModel.WebSiteAccountTokenInfo
    {
        private WebSiteAccountToken() { }


        /// <summary>
        /// 生成logintoken
        /// </summary>
        /// <returns></returns>
        public static WebSiteAccountToken MakeWebSiteAccountToken(string loginToken, string webSiteMark)
        {
            ///查询是否存在
            WebSiteAccountToken res;

            res = new WebSiteAccountToken();
            res.LoginToken = loginToken;
            res.CreateTime = DateTime.Now;

            res.WebSiteMark = webSiteMark;

            var b1 = false;

            for (int i = 0; i < 20 && !b1; i++)
            {
                ///防止token生成重复,尝试10次, 
                res.WebSiteAccountToken = RandomHelper.GetString(25);

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(res);

                b1 = RedisHelper.DBDefault.StringSet(
                    Constant.WebSiteAccountTokenPrefix + res.WebSiteAccountToken
                 , json, TimeSpan.FromHours(5), when: StackExchange.Redis.When.NotExists);
            }

            SqlHelper.Insert<WebSiteAccountTokenInfo>(res).ExecuteAffrows();

            return res;
        }

        //public static List<WebSiteAccountToken> ListWebSiteAccountToken(string loginToken)
        //{
        //    return SqlHelper.Select<WebSiteAccountTokenInfo>()
        //          .Where(w => w.LoginToken == loginToken)
        //          .Include(i => i.WebSite)
        //          .ToList<WebSiteAccountToken>();
        //}

        /// <summary>
        /// 延迟过期
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static bool DelayedExpire(params string[] websiteAccountTokenList)
        {
            foreach (var item in websiteAccountTokenList)
            {
                RedisHelper.DBDefault.KeyExpire(Constant.WebSiteAccountTokenPrefix + item
                   , TimeSpan.FromHours(5));
            }
            return true;
        }


        public static bool DelWebsiteAccountToken(string[] websiteAccountTokenList)
        {
            //var websiteTokenList = SqlHelper.Select<WebSiteAccountTokenInfo>()
            //      .Where(w => w.LoginToken == loginToken)
            //      .ToList();

            var num = SqlHelper.Delete<WebSiteAccountTokenInfo>()
                .Where(w => websiteAccountTokenList.Contains(w.WebSiteAccountToken))
                .ExecuteAffrows();

            if (websiteAccountTokenList.Length > 0)
            {
                var keys = websiteAccountTokenList.Select(s => (RedisKey)(Constant.WebSiteAccountTokenPrefix + s))
                    .ToArray();
                RedisHelper.DBDefault.KeyDelete(keys);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static PageResult<WebSiteAccountToken> PageWebSiteAccountTokenBySecretKey(string webSiteMark, PageParam pageParam)
        {
            var page = SqlHelper.Select<WebSiteAccountToken>()
                  .Where(w => w.WebSiteMark == webSiteMark)
                  .OrderBy(o => o.WebSiteAccountToken)
                  .ToPage(pageParam);

            return page;
        }

        /// <summary>
        /// 续期
        /// </summary>
        /// <param name="url"></param>
        /// <param name="websiteAccountTokenList"></param>
        /// <returns></returns>
        public static ApiMsg RenewalLoginTokenAsync(string url, List<string> websiteAccountTokenList,WebSiteInfo webSite)
        {
            try
            {

                if (websiteAccountTokenList == null || websiteAccountTokenList.Count < 1)
                {
                    return ApiMsg.ReturnSuccess();
                }
                if (string.IsNullOrWhiteSpace(url))
                {
                    return ApiMsg.ReturnSuccess();
                }

                return WebSiteClient.RenewalAccountAsync(url, websiteAccountTokenList, webSite).Result;

            }
            catch (Exception ex)
            {
                LoggerHelper.LogError(ex, "RenewalLoginTokenAsync");
                return ApiMsg.ReturnError(ex.Message);
            }

        }
    }
}

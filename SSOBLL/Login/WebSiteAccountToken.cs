using Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SSOBLL.DBModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class WebSiteAccountToken : DBModel.WebSiteAccountTokenInfo
    {
        private WebSiteAccountToken() { }

        /// <summary>
        /// 生成logintoken
        /// </summary>
        /// <returns></returns>
        public static WebSiteAccountToken GetOrCreateWebSiteAccountToken(string loginToken, string webSiteSecretKey)
        {
            ///查询是否存在
            WebSiteAccountToken res;
            res = DBHelper.Select<WebSiteAccountTokenInfo>().Where(w => w.LoginToken == loginToken
            && w.WebSiteSecretKey == webSiteSecretKey).ToOne<WebSiteAccountToken>();
            if (res != null)
            {
                return res;
            }

            res = new WebSiteAccountToken();
            res.LoginToken = loginToken;
            res.CreateTime = DateTime.Now;

            WebSiteBLL webSiteBll = new WebSiteBLL();
            //var webSite = webSiteBll.GetWebSiteInfoByID(webSiteID);
            //res.WebSiteSecretKey = webSite.WebSiteSecretKey;
            res.WebSiteAccountToken = webSiteSecretKey;

            var b1 = false;

            for (int i = 0; i < 20 && !b1; i++)
            {
                ///防止token生成重复,尝试10次, 
                res.WebSiteAccountToken = RandomHelper.GetString(22);

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(res);

                b1 = RedisHelperStatic.DBDefault.StringSet(Constant.WebSiteAccountTokenPrefix + res.WebSiteAccountToken
                 , json, when: StackExchange.Redis.When.NotExists);

            }

            DBHelper.Insert<WebSiteAccountTokenInfo>(res).ExecuteAffrows();

            return res;

        }

        public static bool DelWebsiteTokenByLoginToken(string loginToken)
        {
            var websiteTokenList = DBHelper.Select<WebSiteAccountTokenInfo>()
                  .Where(w => w.LoginToken == loginToken)
                  .ToList();

            var num = DBHelper.Delete<WebSiteAccountTokenInfo>()
                .Where(w => w.LoginToken == loginToken)
                .ExecuteAffrows();

            if (websiteTokenList.Count > 0)
            {
                var keys = websiteTokenList.Select(s => (RedisKey)(Constant.WebSiteAccountTokenPrefix + s.WebSiteAccountToken))
                    .ToArray();
                RedisHelperStatic.DBDefault.KeyDelete(keys);
            }

            return true;
        }

    }
}

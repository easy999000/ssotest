using Common;
using SSOBLL.DBModel;
using SSOBLL.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class WebSite : WebSiteInfo
    {
        //public static List<WebSiteInfo> GetWebSiteInfoListCatch()
        //{
        //    var res = CatchHelper.GetOrSet(Constant.WebSiteInfoListCatch, GetWebSiteInfoList);
        //    return res;
        //}

        public static List<WebSiteInfo> GetWebSiteInfoList()
        {
            return SqlHelper.SSOCenter.Select<WebSiteInfo>().ToList();
        }

        /// <summary>
        /// 通过 host地址查找对应的 站点信息
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static WebSiteInfo GetWebSiteInfoByHost(string host)
        {
            ///latest.com
            ///l.atest.com
            ///atest.com

            host = "." + host;
            var webList = GetWebSiteInfoList();
            if (webList == null)
            {
                return null;
            }
            foreach (var item in webList)
            {
                if (host.Contains("." + item.WebSiteHost, StringComparison.OrdinalIgnoreCase))
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        ///  
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static WebSiteInfo GetWebSiteInfoByID(int id)
        {
            return SqlHelper.Select<WebSiteInfo>()
                  .Where(w => w.ID == id)
                  .First();
        }

        public static List<WebSite> ListWebSiteByMark(List<string> webSiteMarkList)
        {
            return SqlHelper.Select<WebSiteInfo>()
                  .Where(w => webSiteMarkList.Contains(w.WebSiteMark))
                  .ToList<WebSite>();
        }

        public static WebSiteSecret GetWebSiteSecret_Catch(string webSiteMark)
        {
            return CatchHelper.GetOrSet(Constant.WebSiteSecretPrefix + webSiteMark, () =>
            { 
                var webSite = SqlHelper.Select<WebSiteInfo>()
                       .Where(w => w.WebSiteMark == webSiteMark)
                       .First(); 

                return FreeSqlHelperStatic.Select<WebSiteSecret>()
                  .Where(w => w.WebSiteID == webSite.ID)
                  .ToOne();
            });
        }

        public static bool CreateWebSiteSecret(int webSiteId)
        {
            var key1 = JwtHelper.MakeHS265Key();

            WebSiteSecret webSiteSecret = new WebSiteSecret();
            webSiteSecret.WebSiteID = webSiteId;
            webSiteSecret.Key1 = key1;
            webSiteSecret.Key2 = "";

            var count = FreeSqlHelperStatic.Insert(webSiteSecret)
                   .ExecuteAffrows();

            if (count > 0)
            {
                return true;
            }

            return false;
        }
    }
}

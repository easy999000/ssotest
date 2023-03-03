using Common;
using SSOBLL.DBModel;
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
            var webList = GetWebSiteInfoList();
            if (webList == null)
            {
                return null;
            }
            foreach (var item in webList)
            {
                if (host.Contains(item.WebSiteHost, StringComparison.OrdinalIgnoreCase))
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
            //var webList = GetWebSiteInfoListCatch();
            //if (webList == null)
            //{
            //    return null;
            //}

            //var webSite = webList.Where(w => w.ID == id).FirstOrDefault();

            //return webSite;
        }

        public static List<WebSite> ListWebSiteBySecretKey(List<string> secretKeys)
        {
            return SqlHelper.Select<WebSiteInfo>()
                  .Where(w => secretKeys.Contains(w.WebSiteSecretKey))
                  .ToList<WebSite>();
        }
    }
}

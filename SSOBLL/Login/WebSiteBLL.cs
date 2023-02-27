using Common;
using SSOBLL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class WebSiteBLL
    {
        public List<WebSiteInfo> GetWebSiteInfoListCatch()
        {
            var res = CatchHelper.GetOrSet(Constant.WebSiteInfoListCatch, GetWebSiteInfoList);
            return res;
        }

        public List<WebSiteInfo> GetWebSiteInfoList()
        {
            return DBHelper.SSOCenter.Select<WebSiteInfo>().ToList();
        }

        /// <summary>
        /// 通过 host地址查找对应的 站点信息
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public WebSiteInfo GetWebSiteInfoByHost(string host)
        {
            var webList = GetWebSiteInfoListCatch();
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
        /// 通过 host地址查找对应的 站点信息
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public WebSiteInfo GetWebSiteInfoByID(int id)
        {
            var webList = GetWebSiteInfoListCatch();
            if (webList == null)
            {
                return null;
            }

            var webSite = webList.Where(w => w.ID == id).FirstOrDefault();

            return webSite;
        }

    }
}

using Common;
using SSOBLL.DBModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.JWT
{
    public class WebSiteSecretBLL
    {
        public static WebSiteSecret GetWebSiteSecret(int webSiteId)
        {
            return CatchHelper.GetOrSet(Constant.WebSiteSecretPrefix + webSiteId, () =>
            {
                return FreeSqlHelperStatic.Select<WebSiteSecret>()
                  .Where(w => w.WebSiteID == webSiteId)
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

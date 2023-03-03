using FreeSql.DatabaseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace SSOBLL.DBModel
{

    /// <summary>
    /// sso支持的站点信息表. 
    /// </summary> 
    public partial class WebSiteInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty,Navigate(nameof(WebSiteInfoRoleRelation.WebSiteID))]
        public virtual List<WebSiteInfoRoleRelation> WebSiteRoleRelation { get; set; }

    }

}

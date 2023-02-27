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
    /// 角色和可以登入站点,关联表
    /// </summary> 
    public partial class WebSiteInfoRoleRelation
    { 
        public virtual SSORole Role { get; set; }

        [Navigate(nameof(WebSiteID))]
        public virtual WebSiteInfo WebSiteInfo { get; set; }

    }

}

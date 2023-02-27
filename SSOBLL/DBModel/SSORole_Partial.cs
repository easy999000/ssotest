using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace SSOBLL.DBModel {

	/// <summary>
	/// sso角色,角色是账号和可以登入的站点的纽带, 账号绑定角色,角色绑定站点登入权限
	/// </summary> 
	public partial class SSORole {

		[Navigate(nameof(WebSiteInfoRoleRelation.RoleID))]
		public virtual List<WebSiteInfoRoleRelation> WebSiteRoleRelation { get; set; }

	}

}

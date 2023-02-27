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
	/// 角色和可以登入站点,关联表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class WebSiteInfoRoleRelation {

		[JsonProperty, Column(DbType = "int", IsPrimary = true, IsIdentity = true)]
		public int ID { get; set; }

		[JsonProperty, Column(DbType = "int")]
		public int RoleID { get; set; }

		[JsonProperty, Column(DbType = "int")]
		public int WebSiteID { get; set; }

	}

}

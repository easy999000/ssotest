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
	/// 站点秘钥
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class WebSiteSecret {

		[JsonProperty, Column(DbType = "int", IsPrimary = true)]
		public int WebSiteID { get; set; }

		[JsonProperty, Column(StringLength = 45)]
		public string Key1 { get; set; }

		[JsonProperty, Column(StringLength = 45)]
		public string Key2 { get; set; }

	}

}

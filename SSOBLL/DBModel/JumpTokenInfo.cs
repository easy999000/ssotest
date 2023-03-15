using FreeSql.DatabaseModel;using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FreeSql.DataAnnotations;

namespace SSOBLL.DBModel {

	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class JumpTokenInfo {

		[JsonProperty, Column(StringLength = 50, IsPrimary = true, IsNullable = false)]
		public string JumpToken { get; set; }

		[JsonProperty, Column(DbType = "timestamp", InsertValueSql = "CURRENT_TIMESTAMP")]
		public DateTime CreateTime { get; set; }

		[JsonProperty, Column(StringLength = 50, IsNullable = false)]
		public string WebSiteAccountToken { get; set; }

		[JsonProperty, Column(StringLength = 50, IsNullable = false)]
		public string WebSiteMark { get; set; }

	}

}

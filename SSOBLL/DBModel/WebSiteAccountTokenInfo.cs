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
	/// 站点用户令牌对照表
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class WebSiteAccountTokenInfo {

		[JsonProperty, Column(StringLength = 50, IsPrimary = true, IsNullable = false)]
		public string WebSiteAccountToken { get; set; }

		[JsonProperty, Column(DbType = "timestamp", InsertValueSql = "CURRENT_TIMESTAMP")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 用户在线令牌
		/// </summary>
		[JsonProperty, Column(StringLength = 30, IsNullable = false)]
		public string LoginToken { get; set; }

		/// <summary>
		/// 站点标识
		/// </summary>
		[JsonProperty, Column(StringLength = 60, IsNullable = false)]
		public string WebSiteMark { get; set; }

	}

}

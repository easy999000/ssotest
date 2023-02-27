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
	/// 在线账号信息
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class LoginTokenInfo {

		[JsonProperty, Column(StringLength = 30, IsPrimary = true, IsNullable = false)]
		public string LoginToken { get; set; }

		/// <summary>
		/// 账号
		/// </summary>
		[JsonProperty, Column(StringLength = 45, IsNullable = false)]
		public string Account { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[JsonProperty, Column(DbType = "timestamp", InsertValueSql = "CURRENT_TIMESTAMP")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 用户角色
		/// </summary>
		[JsonProperty, Column(DbType = "int")]
		public int SSORoleID { get; set; }

		/// <summary>
		/// 用户姓名
		/// </summary>
		[JsonProperty, Column(StringLength = 45, IsNullable = false)]
		public string UserName { get; set; }

	}

}

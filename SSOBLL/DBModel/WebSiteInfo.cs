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
	/// sso支持的站点信息表. 
	/// </summary>
	[JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true)]
	public partial class WebSiteInfo {

		[JsonProperty, Column(DbType = "int", IsPrimary = true, IsIdentity = true)]
		public int ID { get; set; }

		/// <summary>
		/// 用户退出接口
		/// </summary>
		[JsonProperty, Column(StringLength = 200, IsNullable = false)]
		public string LogoutApi { get; set; }

		/// <summary>
		/// 用户超时延期接口
		/// </summary>
		[JsonProperty, Column(StringLength = 200, IsNullable = false)]
		public string RenewalApi { get; set; }

		/// <summary>
		/// 视图名字,不同站点登入页面显示的视图名字.
		/// </summary>
		[JsonProperty, Column(StringLength = 45, IsNullable = false)]
		public string ViewName { get; set; }

		/// <summary>
		/// 站点主域名,不包含http,不包含其他部分,只是主域名.
		/// </summary>
		[JsonProperty, Column(StringLength = 100, IsNullable = false)]
		public string WebSiteHost { get; set; }

		[JsonProperty, Column(StringLength = 45, IsNullable = false)]
		public string WebSiteName { get; set; }

		/// <summary>
		/// 站点秘钥
		/// </summary>
		[JsonProperty, Column(StringLength = 60, IsNullable = false)]
		public string WebSiteSecretKey { get; set; }

	}

}

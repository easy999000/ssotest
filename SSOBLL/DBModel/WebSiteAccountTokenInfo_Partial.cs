﻿using FreeSql.DatabaseModel;using System;
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
	public partial class WebSiteAccountTokenInfo {


		[Navigate(nameof(WebSiteInfo.WebSiteSecretKey), TempPrimary = nameof(WebSiteSecretKey))]
		public virtual WebSiteInfo WebSite { get; set; }
	}

}

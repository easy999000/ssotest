using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public  class WebsiteAccountDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public int WebSiteID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WebSiteSecretKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WebSiteAccountToken { get; set; }
    }
}

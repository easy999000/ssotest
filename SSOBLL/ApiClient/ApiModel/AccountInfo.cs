using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.ApiClient.ApiModel
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountInfo
    {
        public string  Account { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string  Mobile { get; set; }
        /// <summary>
        /// 1是管理员
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RoleID { get; set; }

    }
}

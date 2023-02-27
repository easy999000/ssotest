using Common;
using SSOBLL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class Role
    {
        public SSORole RoleInfo { get; set; }
        public List<WebSiteInfo> RelationWebSiteInfo { get; set; }
        = new List<WebSiteInfo>();

        public bool CanWebSite(int WebSiteID)
        {
            if (RelationWebSiteInfo.Any(a => a.ID == WebSiteID))
            {
                return true;
            }

            return false;
        }

        public static Role GetRoleByID(int id)
        {
            var roleList = GetRoleListCatch();
            foreach (var role in roleList)
            {
                if (role.RoleInfo.ID == id)
                {
                    return role;
                }
            }
            return null;
        }
        /// <summary>
        /// 缓存
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoleListCatch()
        {
            var res = CatchHelper.GetOrSet(Constant.RoleAndWebSiteListCatch, GetRoleList);
            return res;
        }
        /// <summary>
        /// 获取角色和站点数据,数据库
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoleList()
        {
            var roleList = DBHelper.SSOCenter.Select<SSORole>()
                .IncludeMany(j => j.WebSiteRoleRelation, then => then.Include(b => b.WebSiteInfo))
                .ToList();

            var res = roleList.Select(s => new Role
            {
                RoleInfo = s
                   ,
                RelationWebSiteInfo = s.WebSiteRoleRelation.Select(x => x.WebSiteInfo).ToList()
            }).ToList();


            return res;
        }
        //public List<Role> 
    }
}

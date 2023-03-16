using Common;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using SSOBLL.DBModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    [JsonObject(MemberSerialization.OptIn), Table(DisableSyncStructure = true,Name =nameof(LoginTokenInfo))]
    public class LoginToken : DBModel.LoginTokenInfo
    {
        private LoginToken() { }

        private List<int> _AllowWebSiteIDs;
        public List<int> AllowWebSiteIDs
        {
            get
            {
                if (_AllowWebSiteIDs == null)
                {
                    Role roleBll = new Role();
                    var role = Role.GetRoleByID(this.SSORoleID);

                    _AllowWebSiteIDs = role.RelationWebSiteInfo.Select(s => s.ID).ToList();

                }
                return _AllowWebSiteIDs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        public List<WebsiteAccountDTO> WebSiteAccountList { get; set; } = new List<WebsiteAccountDTO>();


        /// <summary>
        /// 生成logintoken
        /// </summary>
        /// <returns></returns>
        public static LoginToken MakeLoginToken(string Account, int roleID)
        {
            LoginToken res = new LoginToken();
            res.Account = Account;
            res.CreateTime = DateTime.Now;
            res.UserName = "";
            res.SSORoleID = roleID;
            var b1 = false;

            for (int i = 0; i < 20 && !b1; i++)
            {
                ///防止token生成重复,尝试10次, 
                res.LoginToken = RandomHelper.GetString(20);

                // var json = Newtonsoft.Json.JsonConvert.SerializeObject(res);

                b1 = RedisHelper.DBDefault.Set(Constant.LoginTokenRedisPrefix + res.LoginToken
                 , res, TimeSpan.FromHours(2), when: StackExchange.Redis.When.NotExists);

            }

            SqlHelper.Insert<LoginTokenInfo>(res).ExecuteAffrows();

            return res;
        }

        public void SaveLoginTokenToRedis()
        {
            RedisHelper.DBDefault.Set(Constant.LoginTokenRedisPrefix + LoginToken
             , this, TimeSpan.FromHours(2));
        }


        /// <summary>
        /// 延迟过期
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static bool RenewLoginTokena(string loginToken)
        {
            return RedisHelper.DBDefault.KeyExpire(Constant.LoginTokenRedisPrefix + loginToken, TimeSpan.FromHours(2));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static LoginToken GetLoginTokenByToken(string loginToken)
        {
            var user = RedisHelper.DBDefault.Get<LoginToken>(Constant.LoginTokenRedisPrefix + loginToken);
            return user;
        }

        /// <summary>
        /// 这个方法仅在,redis已经失效的情况下使用.
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static LoginToken GetLoginTokenByTokenFromDB(string loginToken)
        {
            if (loginToken.StartsWith(Constant.LoginTokenRedisPrefix))
            {
                loginToken = loginToken.Substring(Constant.LoginTokenRedisPrefix.Length);
            }
            var loginTokenModel = SqlHelper.Select<LoginTokenInfo>()
                 .Where(w => w.LoginToken == loginToken)
                 .ToOne<LoginToken>();

            if (loginTokenModel == null)
            {
                return null;
            }

            var webAccountList = SqlHelper.Select<WebSiteAccountTokenInfo>()
                  .Where(w => w.LoginToken == loginToken)
                  .Include(w => w.WebSite)
                  .ToList();

            loginTokenModel.WebSiteAccountList = webAccountList.Select(s => new WebsiteAccountDTO
            {
                WebSiteAccountToken = s.WebSiteAccountToken,
                WebSiteID = s.WebSite.ID,
                WebSiteMark = s.WebSiteMark
            }).ToList();


            return loginTokenModel;
        }


        /// <summary>
        /// 
        /// </summary>
        public static bool DelLoginToken(string loginToken)
        {
            var res = true;
            RedisHelper.DBDefault.KeyDelete(Constant.LoginTokenRedisPrefix + loginToken);

            var n1 = SqlHelper.Delete<LoginTokenInfo>(res).Where(w => w.LoginToken == loginToken).ExecuteAffrows();
            if (n1 > 0)
            {
                res = true;
            }
            else
            {
                res = false;
            }
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static bool ExistsLoginTokenRedis(string loginToken)
        {
            return RedisHelper.DBDefault.KeyExists(Constant.LoginTokenRedisPrefix + loginToken);

        }


    }
}

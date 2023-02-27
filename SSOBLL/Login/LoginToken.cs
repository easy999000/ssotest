using Common;
using SSOBLL.DBModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class LoginToken : DBModel.LoginTokenInfo
    {
        private LoginToken() { }

        public List<int> _AllowWebSiteIDs;
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

                b1 = RedisHelperStatic.DBDefault.Set(Constant.LoginTokenRedisPrefix + res.LoginToken
                 , res, TimeSpan.FromHours(2), when: StackExchange.Redis.When.NotExists);

            }

            DBHelper.Insert<LoginTokenInfo>(res).ExecuteAffrows();

            return res;

        }

        /// <summary>
        /// 延迟过期
        /// </summary>
        /// <param name="loginToken"></param>
        /// <returns></returns>
        public static bool DelayedExpire(string loginToken)
        {
            return RedisHelperStatic.DBDefault.KeyExpire(Constant.LoginTokenRedisPrefix + loginToken, TimeSpan.FromHours(2));

        }

        public static LoginToken GetUserInfoByToken(string loginToken)
        {
            var user = RedisHelperStatic.DBDefault.Get<LoginToken>(Constant.LoginTokenRedisPrefix + loginToken);
            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool DelLoginToken(string loginToken)
        {
            var res = RedisHelperStatic.DBDefault.KeyDelete(Constant.LoginTokenRedisPrefix + loginToken);

            DBHelper.Delete<LoginTokenInfo>(res).Where(w => w.LoginToken == loginToken).ExecuteAffrows();

            return res;
        }

    }
}

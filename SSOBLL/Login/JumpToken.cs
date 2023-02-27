using Common;
using SSOBLL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.Login
{
    public class JumpToken : DBModel.JumpTokenInfo
    {
        private JumpToken() { } 

        public static JumpToken MakeJumpToken(string webSiteAccountToken, string webSiteSecretKey)
        {
            JumpToken res = new JumpToken();
            res.WebSiteAccountToken = webSiteAccountToken;
            res.WebSiteSecretKey = webSiteSecretKey;
            res.CreateTime = DateTime.Now;


            var b1 = false;

            for (int i = 0; i < 20 && !b1; i++)
            {
                ///防止token生成重复,尝试10次, 
                res.JumpToken = RandomHelper.GetString(25);

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(res);

                b1 = RedisHelperStatic.DBDefault.StringSet(Constant.JumpTokenRedisPrefix + res.JumpToken
                 , json, expiry: TimeSpan.FromMinutes(3), when: StackExchange.Redis.When.NotExists);

            }
            DBHelper.Insert<JumpTokenInfo>(res).ExecuteAffrows();

            return res;
        }
    }
}

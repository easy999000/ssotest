using SSOBLL.ApiClient.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSOBLL.ApiClient
{
    public class AccountCenterApi
    {
        public AccountCenterApi() { }

        public ApiMsg<AccountInfo> Login(string account, string pass)
        {
            var res =
                       new AccountInfo { Account = account, Name = $"Name{account}"
                       , Mobile = "15999999999", Type = 1,RoleID=41 };
            ///临时模拟
            var str = $"{account},{pass}";
            switch (str)
            {
                case "admin,admin":
                case "test1,test1": 
                    break;
                case "test2,test2":
                case "test3,test3":
                    res.Type = 2;
                    res.RoleID = 42;
                    break;
                default:
                    return ApiMsg<AccountInfo>.ReturnError("账号或密码错误");
            }
            return ApiMsg<AccountInfo>.ReturnSuccess(res);
        }
    }
}

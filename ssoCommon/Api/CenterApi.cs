using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoCommon.Api
{
    public class CenterApi : WebApiClient<ICenterApi>
    {

        public CenterApi(string domain) : base(domain)
        {
        }
    }
    public interface ICenterApi
    {
        [Get("/sso/VerifyToken")]
        Task<MsgInfo<UserInfo>> VerifyToken(string loginID);



    }
}

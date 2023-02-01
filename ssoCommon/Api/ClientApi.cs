using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoCommon.Api
{
    public class ClientApi : WebApiClient<IClientApi>
    {

        public ClientApi(string domain) : base(domain)
        {
        }
    }
    public interface IClientApi
    {
        [Get("/Client/logout")]
        Task<MsgInfo<string>> Logout(string logintoken);



    }
}

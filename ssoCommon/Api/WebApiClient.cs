using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoCommon.Api
{
    public class WebApiClient<T>
    {
        public T Client { get; set; }
        public string Domain { get; set; }

        public WebApiClient(string domain)
        {
            Domain = domain;

            var client = RestService.For<T>(this.Domain);
            Client = client;

        }
    }
}

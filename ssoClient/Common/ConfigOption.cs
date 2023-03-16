using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssoClient.Common
{
    public class ConfigOption
    {
        public string CenterDomain { get; set; }
        public string[] ClientDomain { get; set; }

        public string CurrentDomain { get; set; }

        public static ConfigOption DefaultConfig { get; set; }
    }
}

 
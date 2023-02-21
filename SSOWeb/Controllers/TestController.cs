using Common;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace SSOWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _chche;
        private readonly IConfiguration _config;
        private IDataProtector _protector;
        public TestController(ILogger<HomeController> logger, IDistributedCache chche
            , IConfiguration config, IDataProtectionProvider protector)
        {
            _logger = logger;
            _chche = chche;
            _config = config;
            _protector = protector.CreateProtector("test"); ;
        }
        public IActionResult Index()
        {
            return Content("Index");
        }

        public IActionResult set()
        {

            RedisHelperStatic.DBDefault.StringSet("redistest3", DateTime.Now.ToString(),
                new TimeSpan(0, 10, 0));


            return Content("ok");
        }
        public IActionResult get()
        {
            var str = _chche.GetString("redistest");
            return Content(str);
        }
        public IActionResult Protect(string param="a")
        {
            var str = _protector.Protect(param);
            return Content(str);
        }
        public IActionResult Unprotect(string param)
        {
            var str = _protector.Unprotect(param);
            return Content(str);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;

namespace GUI.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{responseCode}")]
        public IActionResult Index(int? responseCode)
        {
            responseCode = (!responseCode.HasValue || (responseCode != 404 && responseCode != 500)) ? 500 : responseCode;
            return View(responseCode.Value.ToString());
        }
    }
}

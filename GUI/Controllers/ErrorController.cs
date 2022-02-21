using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GUI.Controllers
{
    [Route("/Error")]
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
            _logger.LogInformation($"Error, response code = {responseCode}");
            responseCode = (!responseCode.HasValue || (responseCode != 404 && responseCode != 500)) ? 500 : responseCode;
            return View(responseCode.Value.ToString());
        }
    }
}

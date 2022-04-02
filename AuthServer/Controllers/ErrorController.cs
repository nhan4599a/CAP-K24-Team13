using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        [HttpGet("{resultCode}")]
        public IActionResult Index(int resultCode)
        {
            return View(resultCode.ToString());
        }

        [HttpGet("throw")]
        public IActionResult Throw()
        {
            throw new System.NotImplementedException();
        }
    }
}

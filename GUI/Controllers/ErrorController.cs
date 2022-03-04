using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    [Route("/Error")]
    public class ErrorController : Controller
    {
        [HttpGet("{resultCode}")]
        public IActionResult Index(int resultCode)
        {
            return View(resultCode.ToString());
        }
    }
}

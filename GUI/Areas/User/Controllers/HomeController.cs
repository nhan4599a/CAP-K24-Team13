using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        [ActionName("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

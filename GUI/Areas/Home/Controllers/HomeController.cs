using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Home.Controllers
{
    [Area("Home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
         
        public IActionResult ListProduct()
        {
            return View();
        }
    }
}

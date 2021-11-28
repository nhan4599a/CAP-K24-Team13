using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Product");
        }
    }
}

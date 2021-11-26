using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

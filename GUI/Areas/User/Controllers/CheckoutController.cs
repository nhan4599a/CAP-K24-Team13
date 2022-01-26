using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

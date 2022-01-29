using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class UserAccountController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}

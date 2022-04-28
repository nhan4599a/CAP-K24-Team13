using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}

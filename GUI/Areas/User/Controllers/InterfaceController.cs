using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class InterfaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

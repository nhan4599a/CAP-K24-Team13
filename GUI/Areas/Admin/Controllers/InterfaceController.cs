using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InterfaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

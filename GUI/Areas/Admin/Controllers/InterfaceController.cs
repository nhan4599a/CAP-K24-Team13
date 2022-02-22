using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "ShopOwner")]
    [Area("Admin")]
    public class InterfaceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Edit")]
        public IActionResult EditMode()
        {
            return View("EditMode");
        }
    }
}

using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    public class InterfaceController : BaseShopOwnerController
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

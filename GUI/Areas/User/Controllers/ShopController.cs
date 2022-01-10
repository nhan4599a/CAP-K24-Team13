using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    [Area("User")]
    public class ShopController : Controller
    {
        [ActionName("Index")]
        public IActionResult ShopIndex()
        {
            return View();
        }
    }
}

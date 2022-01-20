using GUI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

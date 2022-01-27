using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.User.Controllers
{
    public class CheckoutController : BaseUserController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

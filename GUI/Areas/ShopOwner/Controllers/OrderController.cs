using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    public class OrderController : BaseShopOwnerController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

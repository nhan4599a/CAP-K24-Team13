using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    public class OrderController : BaseShopOwnerController
    {
        [ActionName("Index")]
        public IActionResult Kanban()
        {
            return View();
        }

        [ActionName("sell-history")]
        public IActionResult SellHistory()
        {
            return View();
        }
    }
}

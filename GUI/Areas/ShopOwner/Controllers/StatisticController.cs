using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.ShopOwner.Controllers
{
    public class StatisticController : BaseShopOwnerController
    {
        [ActionName("Index")]
        public IActionResult Statistic()
        {
            return View();
        }
    }
}

using GUI.Abtractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    public class CategoryController : BaseShopOwnerController
    {
        [ActionName("Index")]
        public IActionResult ListCategory()
        {
            return View();
        }
    }
}

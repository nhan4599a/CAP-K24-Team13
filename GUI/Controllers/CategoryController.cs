using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class CategoryController : Controller
    {
        [ActionName("Add")]
        public IActionResult AddCategories()
        {       
            return View();
        }
    }
}
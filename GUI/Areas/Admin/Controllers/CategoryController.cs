using Microsoft.AspNetCore.Mvc;

namespace GUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        [ActionName("Add")]
        public IActionResult AddCategory()
        {
            return View();
        } 

        [ActionName("Index")]
        public IActionResult ListCategory()
        {
            return View();
        }

        [ActionName("Edit")]
        public IActionResult EditCategory()
        {
            return View();
        }
    }
}

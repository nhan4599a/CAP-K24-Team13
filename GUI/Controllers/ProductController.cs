using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult AddProduct()
        {

            return View();
        }
        public IActionResult AddCategories()
        {
            return View();
        }
    }

}
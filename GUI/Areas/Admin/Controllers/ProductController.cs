using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class ProductController : Controller
    {
        [ActionName("Add")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [ActionName("Index")]
        public IActionResult ListProduct()
        {
            return View();
        }

        [ActionName("Edit")]
        public IActionResult EditProduct()
        {
            return View();
        }
    }
}
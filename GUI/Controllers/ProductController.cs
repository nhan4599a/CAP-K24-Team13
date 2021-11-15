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
<<<<<<< HEAD
        public IActionResult AddCategories()
        {
            return View();
        }

        [ActionName("Index")]
        public IActionResult ListProduct()
        {
            return View();
        }
=======
     

>>>>>>> 4ba5647 (update)
    }

}
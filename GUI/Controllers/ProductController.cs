using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    public class ProductController : Controller
    {
        [ActionName("Add")]
        public IActionResult AddProduct()
        {
<<<<<<< HEAD
=======
            

>>>>>>> 5dc9564 (UpdateFormAddProduct)
            return View();
        }

        [ActionName("Index")]
        public IActionResult ListProduct()
        {
            return View();
        }
    }
}

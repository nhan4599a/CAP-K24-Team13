using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> EditProduct()
        {
            return View();
        }
    }
}

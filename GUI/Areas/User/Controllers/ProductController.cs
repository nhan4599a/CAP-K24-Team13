using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Area("User")]
    public class ProductController : Controller
    {
        private readonly IProductClient _productClient;

        public ProductController(IProductClient productClient)
        {
            _productClient = productClient;
        }
        public IActionResult ListProduct()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }
    }
}
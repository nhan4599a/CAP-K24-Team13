using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Area("User")]
    public class ShopController : Controller
    {
        private readonly IProductClient _productClient;
        public ShopController(IProductClient productClient)
        {
            _productClient = productClient;
        }
        public async Task<IActionResult> Index()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }
        public IActionResult ListProduct()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly IProductClient _productClient;

        public HomeController(IProductClient productClient)
        {
            _productClient = productClient;
        }
        
        public async Task<IActionResult> Index()
        {
            var products = await _productClient.GetProductsAsync(1, 5);
            return View();
        }

        public IActionResult ListProduct()
        {
            return View();
        }
    }
}

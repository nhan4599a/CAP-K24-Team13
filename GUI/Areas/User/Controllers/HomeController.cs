using Microsoft.AspNetCore.Mvc;

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
        
        public IActionResult Index()
        {
            var products = _productClient.GetProductsAsync(1, 5);
            return View();
        }
        public IActionResult ListProduct()
        {
            return View();
        }
    }
}

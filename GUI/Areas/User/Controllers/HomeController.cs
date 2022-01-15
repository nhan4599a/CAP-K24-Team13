using GUI.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class HomeController : Controller
    {
        private readonly IProductClient _productClient;

        public HomeController(IProductClient productClient)
        {
            _productClient = productClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }
    }
}

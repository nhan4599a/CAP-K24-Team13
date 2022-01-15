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

        public async Task<IActionResult> Index(string id)
        {
            var result = await _productClient.GetProductAsync(id);
            if (!result.IsSuccessStatusCode || result.Content.ResponseCode == 404)
            {
                return StatusCode((int)result.StatusCode);
            }
            var product = result.Content.Data;
            return View(product);
        }
    }
}
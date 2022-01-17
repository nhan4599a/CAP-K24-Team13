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
        public async Task<IActionResult> ProductbyCategory()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }

        public async Task<IActionResult> ProductbyCategory1()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }
        public async Task<IActionResult> ProductbyCategory2()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }
        public async Task<IActionResult> ProductbyCategory3()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
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
        //public async Task<IActionResult> Productbycategory(string id)
        //{
        //    var result = await _productClient.GetProductAsync(id);
        //    if (!result.IsSuccessStatusCode || result.Content.ResponseCode == 404)
        //    {
        //        return StatusCode((int)result.StatusCode);
        //    }
        //    var product = result.Content.Data;
        //    return View(product);
        //}
    }
}
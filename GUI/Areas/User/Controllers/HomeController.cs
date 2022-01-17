using GUI.Areas.User.ViewModels;
using GUI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    [VirtualArea("User")]
    public class HomeController : Controller
    {
        private readonly IProductClient _productClient;
        private readonly IShopClient _shopClient;

        public HomeController(IProductClient productClient, IShopClient shopClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = (await _productClient.GetProductsAsync(1, 5)).Data;
            return View(products);
        }

        public async Task<IActionResult> Search(string keyword, int pageNumber, int? pageSize)
        {
            var productResponse = await _productClient.FindProducts(keyword, pageNumber, pageSize);
            var shopResponse = await _shopClient.FindShops(keyword, pageNumber, pageSize);
            if (!productResponse.IsSuccessStatusCode || !shopResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            ViewBag.Keyword = keyword;
            return View(new SearchResultViewModel
            {
                Products = productResponse.Content.Data,
                Shops = shopResponse.Content
            });
        }
    }
}

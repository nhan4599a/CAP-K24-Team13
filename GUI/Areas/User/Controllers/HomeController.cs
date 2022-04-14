using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class HomeController : BaseUserController
    {
        private readonly IProductClient _productClient;
        private readonly IExternalShopClient _shopClient;
        private readonly ICategoryClient _categoryClient;

        public HomeController(IProductClient productClient, IShopClient shopClient, ICategoryClient categoryClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
            _categoryClient = categoryClient;
        }

        public async Task<IActionResult> Index()
        {
            var bestSellerProductsResponseTask = _productClient.GetBestSellerProducts(null);
            var topMostSaleOffProductsResponseTask = _productClient.GetMostSaleOffProducts();
            var categoriesResponseTask = _categoryClient.GetCategoriesOfShop
            var shopsResponseTask = _shopClient.GetAllShops();
            var bestSellerProductsResponse = await bestSellerProductsResponseTask;
            var topMostSaleOffProductsResponse = await topMostSaleOffProductsResponseTask;
            var shopsResponse = await shopsResponseTask;
            if (!bestSellerProductsResponse.IsSuccessStatusCode || !shopsResponse.IsSuccessStatusCode
                    || !topMostSaleOffProductsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return View(new HomePageViewModel
            {
                Shops = shopsResponse.Content.Select(shop => (shop.Id, shop.ShopName)).ToList(),
                BestSellerProducts = bestSellerProductsResponse.Content.Data,
                TopMostSaleOffProducts = topMostSaleOffProductsResponse.Content.Data
            });
        }

        public async Task<IActionResult> Search(string cat, string keyword, int pageNumber, int pageSize = 5)
        {
            if (cat.ToLower() != "product" && cat.ToLower() != "shop")
                return StatusCode(StatusCodes.Status404NotFound);
            ViewBag.Keyword = keyword;
            ViewBag.PageSize = pageSize;
            ViewBag.Cat = cat;
            if (cat.ToLower() == "product")
            {
                var productResponse = await _productClient.FindProducts(keyword, pageNumber, pageSize);
                if (!productResponse.IsSuccessStatusCode)
                    return StatusCode(StatusCodes.Status500InternalServerError);
                return View(new SearchResultViewModel
                {
                    Products = productResponse.Content.Data,
                    Shops = null
                });
            }
            else
            {
                var shopResponse = await _shopClient.FindShops(keyword, pageNumber, pageSize);
                if (!shopResponse.IsSuccessStatusCode)
                    return StatusCode(StatusCodes.Status500InternalServerError);
                return View(new SearchResultViewModel
                {
                    Products = null,
                    Shops = shopResponse.Content.ToInternal()
                });
            }
        }
    }
}

using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class ShopController : BaseUserController
    {
        private readonly IProductClient _productClient;
        private readonly IShopClient _shopClient;
        private readonly ICategoryClient _categoryClient;

        public ShopController(IProductClient productClient, IShopClient shopClient,
            ICategoryClient categoryClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
            _categoryClient = categoryClient;
        }

        public async Task<IActionResult> Index(int id)
        {
            var bestSellerProductsResponseTask = _productClient.GetBestSellerProducts(id);
            var shopCategoriesResponseTask = _categoryClient.GetCategoriesOfShop(id, 4);
            var shopResponseTask = _shopClient.GetShop(id);
            var bestSellerProductsResponse = await bestSellerProductsResponseTask;
            var shopCategoriesResponse = await shopCategoriesResponseTask;
            if (!bestSellerProductsResponse.IsSuccessStatusCode || !shopCategoriesResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var productsResponse = await _productClient.GetProductsOfCategory(shopCategoriesResponse.Content.Data.Select(e => e.Id).ToArray(), 1, 0);
            if (!productsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var productsOfCategory = productsResponse.Content.Data.Data
                .GroupBy(e => e.CategoryName)
                .ToDictionary(e => shopCategoriesResponse.Content.Data.Data.First(item => item.CategoryName == e.Key).Id,
                    e => e.Take(5).ToList());
            var shopResponse = await shopResponseTask;
            if (!shopResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return View(new ShopDetailViewModel
            {
                Categories = shopCategoriesResponse.Content.Data.ToList(),
                Shop = shopResponse.Content.ResultObj,
                BestSeller = bestSellerProductsResponse.Content.Data,
                Products = productsOfCategory
            });
        }

        public async Task<IActionResult> Categories(int id, [FromQuery(Name = "cat")] List<int> categoryId, int pageNumber = 1)
        {
            var shopCategoriesResponseTask = _categoryClient.GetCategoriesOfShop(id, 0);
            var shopResponseTask = _shopClient.GetShop(id);
            var productsResponseTask = _productClient.GetProductsOfCategory(categoryId.ToArray(), pageNumber, 20);
            var shopCategoriesResponse = await shopCategoriesResponseTask;
            var productsResponse = await productsResponseTask;
            if (!shopCategoriesResponse.IsSuccessStatusCode
                    || !productsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var shopResponse = await shopResponseTask;
            if (!shopResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return View(new ShopCategoryViewModel
            {
                Categories = shopCategoriesResponse.Content.Data.ToList(),
                Shop = shopResponse.Content.ResultObj,
                Products = productsResponse.Content.Data
            });
        }
    }
}

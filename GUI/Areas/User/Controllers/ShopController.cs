using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
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

        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
                return StatusCode(StatusCodes.Status404NotFound);
            var bestSellerProductsResponseTask = _productClient.GetBestSellerProducts(id.Value);
            var shopResponseTask = _shopClient.GetShop(id.Value);
            var bestSellerProductsResponse = await bestSellerProductsResponseTask;
            var shopResponse = await shopResponseTask;
            if (bestSellerProductsResponse.IsSuccessStatusCode && shopResponse.IsSuccessStatusCode)
            {
                if (bestSellerProductsResponse.Content.Data.Any())
                {
                    var shopCategoriesResponseTask = _categoryClient.GetCategoriesOfShop(id.Value, 4);
                    var shopCategoriesResponse = await shopCategoriesResponseTask;
                    var productsResponse = await _productClient.GetProductsOfShopInCategory(
                        id.Value, shopCategoriesResponse.Content.Data.Select(e => e.CategoryId).ToArray(), string.Empty, 1, 0
                    );
                    var categories = shopCategoriesResponse.Content.Data.ToList();
                    categories.Insert(0, new CategoryDTO
                    {
                        CategoryId = int.MinValue,
                        CategoryName = "All",
                        ProductCount = productsResponse.Content.Data.ToList().Take(20).Count()
                    });
                    var productsOfCategory = productsResponse.Content.Data.Data
                        .GroupBy(e => e.CategoryName)
                        .ToDictionary(
                            e => shopCategoriesResponse.Content.Data.Data.First(item => item.CategoryName == e.Key).CategoryId,
                            e => e.Take(5).ToList());
                    foreach (var category in shopCategoriesResponse.Content.Data.Data)
                    {
                        if (!productsOfCategory.ContainsKey(category.CategoryId))
                            productsOfCategory.Add(category.CategoryId, new List<ProductDTO>());
                    }
                    productsOfCategory.Add(int.MinValue, productsResponse.Content.Data.ToList().Take(20).ToList());
                    return View(new ShopDetailViewModel
                    {
                        BestSeller = bestSellerProductsResponse.Content.Data,
                        Products = productsOfCategory,
                        Categories = categories,
                        Shop = shopResponse.Content.ResultObj
                    });
                }
                else
                {
                    return View(new ShopDetailViewModel
                    {
                        Categories = new List<CategoryDTO>(),
                        BestSeller = new List<MinimalProductDTO>(),
                        Products = new Dictionary<int, List<ProductDTO>>(),
                        Shop = shopResponse.Content.ResultObj
                    });
                }    
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<IActionResult> Categories(int? id, [FromQuery(Name = "cat")] List<int> categoryId, [FromQuery(Name = "q")] string keyword, int pageNumber = 1)
        {
            if (!id.HasValue)
                return StatusCode(StatusCodes.Status404NotFound);
            var shopCategoriesResponseTask = _categoryClient.GetCategoriesOfShop(id.Value, 0);
            var shopResponseTask = _shopClient.GetShop(id.Value);
            var productsResponseTask = _productClient.GetProductsOfShopInCategory(id.Value, categoryId.ToArray(), keyword, pageNumber, 20);
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

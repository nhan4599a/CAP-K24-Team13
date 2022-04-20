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
    public class HomeController : BaseUserController
    {
        private readonly IProductClient _productClient;
        private readonly IExternalShopClient _shopClient;
        private readonly ICategoryClient _categoryClient;

        private readonly string[] HomePageCategoriesName =
        {
            "Fashion", "Electronics", "Furniture", "Book & Magazine"
        };

        private readonly int[][] HomePageCategoriesId =
        {
            new int[]
            {
                1, 2, 5, 7, 8, 11, 12, 13, 18
            },
            new int[]
            {
                6, 9, 13, 14, 19, 20, 28
            },
            new int[]
            {
                21
            },
            new int[]
            {
                27
            }
        };

        private readonly string[][] HomePageChildCategoriesName =
        {
            new string[]
            {
                "Women Fashion", "Men Fashion", "Fashion Accessories", "Men's Shoes", "Women's Shoes", "Women's bag",
                "Men's bag", "Watches", "Baby fashion"
            },
            new string[]
            {
                "Electrics", "Phone & accessories", "Watches", "Speaker Devices", "Gaming & Console", "Camera & Flycam",
                "Computer & Laptop"
            },
            new string[]
            {
                "House & Life"
            },
            new string[]
            {
                "Book & Magazine"
            }
        };

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
            var shopsResponseTask = _shopClient.GetAllShops();
            var bestSellerProductsResponse = await bestSellerProductsResponseTask;
            var topMostSaleOffProductsResponse = await topMostSaleOffProductsResponseTask;
            var shopsResponse = await shopsResponseTask;
            if (!bestSellerProductsResponse.IsSuccessStatusCode || !shopsResponse.IsSuccessStatusCode
                    || !topMostSaleOffProductsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var productsResponse = await 
                _productClient.GetProductsInCategory(HomePageCategoriesId.SelectMany(i => i).Distinct().ToArray(), 1, 0);
            if (!productsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var productsInCategories = new Dictionary<string, List<ProductDTO>>();
            for (int i = 0; i < HomePageCategoriesId.Length; i++)
            {
                productsInCategories.Add(
                    HomePageCategoriesName[i],
                    productsResponse.Content.Data.Data
                        .Where(e => HomePageChildCategoriesName[i].Contains(e.CategoryName)).Take(5).ToList()
                );
            }
            productsInCategories.Add("All",
                    productsResponse.Content.Data.Data.Take(20).ToList());
            return View(new HomePageViewModel
            {
                Shops = shopsResponse.Content.Select(shop => (shop.Id, shop.ShopName)).ToList(),
                BestSellerProducts = bestSellerProductsResponse.Content.Data,
                TopMostSaleOffProducts = topMostSaleOffProductsResponse.Content.Data,
                Products = productsInCategories
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

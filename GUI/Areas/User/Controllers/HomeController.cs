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
            "Fashion", "Electronics", "Furniture", "Book & Magazine", "Convenience", "Accessories"
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
            },
            new int[]
            {
                6, 9, 10, 14, 16, 21
            },
            new int[]
            {
                1, 2, 3, 4, 5, 7, 8, 11, 12, 13
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
            },
            new string[]
            {
                "Electronics", "Phone & accessories", "Travel & Luggage", "Speaker Devices", "Pet care", "House & Life"
            },
            new string[]
            {
                "Women Fashion", "Men Fashion", "Beauty", "Health", "Fashion Accessories", "Men's Shoes", "Women's Shoes",
                "Women's bag", "Men's bag", "Watches"
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
            var newProductsResponseTask = _productClient.GetTopNewsProducts();
            var bestSellerProductsResponse = await bestSellerProductsResponseTask;
            var topMostSaleOffProductsResponse = await topMostSaleOffProductsResponseTask;
            var shopsResponse = await shopsResponseTask;
            if (!bestSellerProductsResponse.IsSuccessStatusCode || !shopsResponse.IsSuccessStatusCode
                    || !topMostSaleOffProductsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var productsResponse = await 
                _productClient.GetProductsInCategory(HomePageCategoriesId.SelectMany(i => i).Distinct().ToArray(), "", 1, 0);
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
            var newProductsResponse = await newProductsResponseTask;
            if (newProductsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return View(new HomePageViewModel
            {
                Shops = shopsResponse.Content.Where(shop => shop.IsAvailable).Select(shop => (shop.Id, shop.ShopName)).ToList(),
                BestSellerProducts = bestSellerProductsResponse.Content.Data,
                NewProducts = newProductsResponse.Content.Data,
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

        public async Task<IActionResult> Categories([FromQuery(Name = "q")] string keyword, [FromQuery(Name = "cat")] List<int> categoryId)
        {
            var categoriesResponseTask = _categoryClient.GetCategories(0);
            var productsResponseTask = _productClient.GetProductsInCategory(categoryId.ToArray(), keyword, 1, 20);
            var categoriesResponse = await categoriesResponseTask;
            var productsResponse = await productsResponseTask;
            if (!categoriesResponse.IsSuccessStatusCode || !productsResponse.IsSuccessStatusCode)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return View(new HomeCategoryViewModel
            {
                Categories = categoriesResponse.Content.Data.ToList(),
                Products = productsResponse.Content.Data
            });
        }

        public IActionResult About()
        {
            return View();
        }
    }
}

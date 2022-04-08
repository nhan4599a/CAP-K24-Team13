using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class HomeController : BaseUserController
    {
        private readonly IProductClient _productClient;
        private readonly IShopClient _shopClient;

        public HomeController(IProductClient productClient, IShopClient shopClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
        }

        public IActionResult Index()
        {
            return View();
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

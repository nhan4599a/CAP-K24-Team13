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
        private readonly IShopInterfaceClient _interfaceClient;

        public HomeController(IProductClient productClient, IShopClient shopClient, IShopInterfaceClient interfaceClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
            _interfaceClient = interfaceClient;
        }

        public IActionResult Index()
        {
            //var productsResponse = await _productClient.GetProductsAsync(1, 5);
            //var cartItemCountResponse = await _cartClient.GetCartItemCountAsync("F081C3C0-3314-44D8-1055-08D9DA433EEF");
            //if (!productsResponse.IsSuccessStatusCode)
            //    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            //ViewBag.CartItemCount = cartItemCountResponse.IsSuccessStatusCode ? cartItemCountResponse.Content : 0;
            //return View(productsResponse.Content.Data);
            return View();
        }

        public async Task<IActionResult> Search(string keyword, int pageNumber, int pageSize = 5)
        {
            var productResponse = await _productClient.FindProducts(keyword, pageNumber, pageSize);
            var shopResponse = await _shopClient.FindShops(keyword, pageNumber, pageSize);
            if (!productResponse.IsSuccessStatusCode || !shopResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            ViewBag.Keyword = keyword;
            ViewBag.PageSize = pageSize;
            return View(new SearchResultViewModel
            {
                Products = productResponse.Content.Data,
                Shops = shopResponse.Content.ToInternal()
            });
        }
    }
}

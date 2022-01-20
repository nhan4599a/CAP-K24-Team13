using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductClient _productClient;
        private readonly IShopClient _shopClient;
        private readonly ICategoryClient _categoryClient;
        private readonly ICartClient _cartClient;

        public ShopController(IProductClient productClient, IShopClient shopClient,
            ICategoryClient categoryClient, ICartClient cartClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
            _categoryClient = categoryClient;
            _cartClient = cartClient;
        }

        public async Task<IActionResult> Index(int id)
        {
            var productResponse = await _productClient.GetProductsOfShopAsync(id);
            var informationResponse = await _shopClient.FindInformation(id);
            var shopResponse = await _shopClient.GetShop(id);
            var categoryResponse = await _categoryClient.GetCategoriesOfShop(id);
            var cartItemCountResponse = await _cartClient.GetCartItemCountAsync("F081C3C0-3314-44D8-1055-08D9DA433EEF");
            if (!productResponse.IsSuccessStatusCode ||
                !informationResponse.IsSuccessStatusCode ||
                !categoryResponse.IsSuccessStatusCode ||
                shopResponse.ResponseCode != 200)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            ViewBag.CartItemCount = cartItemCountResponse.IsSuccessStatusCode ? cartItemCountResponse.Content : 0;
            return View(new ShopDetailViewModel
            {
                Products = productResponse.Content.Data,
                Information = informationResponse.Content.Data,
                Categories = categoryResponse.Content.Data,
                Shop = shopResponse.Data
            });
        }
    }
}

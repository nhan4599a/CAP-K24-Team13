using GUI.Abtractions;
using GUI.Areas.User.ViewModels;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var productResponse = await _productClient.GetProductsOfShopAsync(id);
            var shopResponse = await _shopClient.GetShop(id);
            var categoryResponse = await _categoryClient.GetCategoriesOfShop(id);
            if (!productResponse.IsSuccessStatusCode ||
                !categoryResponse.IsSuccessStatusCode ||
                !shopResponse.IsSuccessStatusCode)
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(new ShopDetailViewModel
            {
                Products = productResponse.Content.Data,
                Categories = categoryResponse.Content.Data,
                Shop = shopResponse.Content.ResultObj
            });
        }
    }
}

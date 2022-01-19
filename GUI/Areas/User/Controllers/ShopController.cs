using GUI.Areas.User.ViewModels;
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

        public ShopController(IProductClient productClient, IShopClient shopClient, ICategoryClient categoryClient)
        {
            _productClient = productClient;
            _shopClient = shopClient;
            _categoryClient = categoryClient;
        }

        public async Task<IActionResult> Index(int id)
        {
            var productResponse = await _productClient.GetProductsOfShopAsync(id);
            var informationResponse = await _shopClient.FindInformation(id);
            var categoryResponse = await _categoryClient.GetCategoriesOfShop(id);
            if (!productResponse.IsSuccessStatusCode || !informationResponse.IsSuccessStatusCode || !categoryResponse.IsSuccessStatusCode)
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            return View(new ShopDetailViewModel
            {
                Products = productResponse.Content.Data,
                Information = informationResponse.Content.Data,
                Categories = categoryResponse.Content.Data
            });
        }
    }
}

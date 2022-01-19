using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductClient _productClient;

        public ShopController(IProductClient productClient)
        {
            _productClient = productClient;
        }

        public async Task<IActionResult> Index(int id)
        {
            //var response = await _productClient.GetProductsOfShopAsync(id);
            //if (!response.IsSuccessStatusCode)
            //    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            //return View(response.Content.Data);
            return View();
        }
    }
}

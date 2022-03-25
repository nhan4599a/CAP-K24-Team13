using GUI.Abtractions;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System.Threading.Tasks;

namespace GUI.Areas.User.Controllers
{
    public class BrandsController : BaseUserController
    {
        private readonly IShopClient _shopClient;

        public BrandsController(IShopClient shopClient)
        {
            _shopClient = shopClient;
        }

        public async Task<IActionResult> Index([FromQuery] string keyword)
        {
            var shopsResponse = await _shopClient.GetAllShops();
            if (!shopsResponse.IsSuccessStatusCode)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(shopsResponse.Content);
        }
    }
}

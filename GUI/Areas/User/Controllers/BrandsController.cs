using GUI.Abtractions;
using GUI.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            var shopsResponse = await 
                (string.IsNullOrWhiteSpace(keyword) 
                    ? _shopClient.GetAllShops() 
                    : _shopClient.FindShops(keyword));
            if (!shopsResponse.IsSuccessStatusCode)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return View(shopsResponse.Content.Where(shop => shop.IsAvailable).ToList());
        }
    }
}

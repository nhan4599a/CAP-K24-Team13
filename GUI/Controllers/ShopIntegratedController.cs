using GUI.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    [ApiController]
    public class ShopIntegratedController : ControllerBase
    {
        private readonly IShopClient _shopClient;

        public ShopIntegratedController(IShopClient shopClient)
        {
            _shopClient = shopClient;
        }

        [HttpGet("api/integrated/shop/{shopId}")]
        public async Task<ApiResult> GetIntegratedShopAsync(int shopId)
        {
            var shop = await _shopClient.GetShop(shopId);

            return ApiResult<ShopDTO>.CreateSucceedResult(shop.Content.ResultObj);
        }
    }
}

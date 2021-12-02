using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using ShopInterfaceService.Service;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopInterfaceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopInterfaceController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopInterfaceController(IShopService shopService)
        {
            _shopService = shopService;
        }
        // GET: api/<ShopController>
        [HttpPut]
        [Route("ShopInterface")]
        public async Task<ApiResult<ShopInterfaceDTO>> EditShopInterface(ShopInterfaceDTO shopInterfaceDTO)
        {
            var result = await _shopService.EditShopInformation(shopInterfaceDTO);
            if (result == null) return new ApiResult<ShopInterfaceDTO> { Data = null, ResponseCode = 405 };
            return new ApiResult<ShopInterfaceDTO> { Data = result, ResponseCode = 200 };
        }
    }
}

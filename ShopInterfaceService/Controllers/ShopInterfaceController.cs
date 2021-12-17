using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using ShopInterfaceService.Commands;
using ShopInterfaceService.Mediator;
using System.Threading.Tasks;

namespace ShopInterfaceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopInterfaceController : ControllerBase
    {
        //private readonly IShopService _shopService;

        //public ShopInterfaceController(IShopService shopService)
        //{
        //    _shopService = shopService;
        //}

        //[HttpPost]
        //[Route("{shopId}")]
        //public async Task<ApiResult<ShopInterfaceDTO>> EditShopInterface(int shopId, AddOrEditShopInterfaceRequestModel requestModel)
        //{
        //    var existedShopInterface = await _shopService.FindShopInterfaceAsync(shopId);
        //    ShopInterfaceDTO result;
        //    if (existedShopInterface == null)
        //        result = await _shopService.AddShopInterfaceAsync(shopId, requestModel);
        //    else
        //        result = await _shopService.EditShopInterfaceAsync(shopId, requestModel);
        //    if (result == null) return new ApiResult<ShopInterfaceDTO> { Data = null, ResponseCode = 404 };
        //    return new ApiResult<ShopInterfaceDTO> { Data = result, ResponseCode = 200 };
        //}
        private readonly IMediator _mediator;

        public ShopInterfaceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<CommandResponse<ShopInterfaceDTO>> Get(string searchString)
        {
            var result = await _mediator.Send(new FindShop.Query { SearchString = searchString });
            return result;
        }

        [HttpPost]
        public async Task<CommandResponse<int>> EditShopInterface(int shopId, 
            CreateOrEditShopInterfaceRequestModel requestModel)
        {
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            return result;
        }
    }
}

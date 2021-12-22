using AspNetCoreSharedComponent;
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
    [Route("api/interfaces")]
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
        private readonly ImageManager _imageManager;

        public ShopInterfaceController(IMediator mediator, ImageManager imageManager)
        {
            _mediator = mediator;
            _imageManager = imageManager;
        }

        [HttpGet]
        public async Task<CommandResponse<ShopInterfaceDTO>> Get(string searchString)
        {
            var result = await _mediator.Send(new FindShop.Query { SearchString = searchString });
            return result;
        }

        [HttpPost("{shopId}")]
        public async Task<ApiResult<bool>> CreateShopInterface(int shopId, 
            CreateOrEditInterfaceRequestModel requestModel)
        {
            requestModel.ShopImages = await _imageManager.SaveFilesAsync(Request.Form.Files);
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, Data = false, ErrorMessage = result.ErrorMessage };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpPut("{shopId}")]
        public async Task<ApiResult<bool>> EditShopInterface(int shopId,
            CreateOrEditInterfaceRequestModel requestModel)
        {
            requestModel.ShopImages = await _imageManager.EditFilesAsync(requestModel.ShopImages, Request.Form.Files);
            var result = await _mediator.Send(new CreateOrEditShopInterfaceCommand
            {
                ShopId = shopId,
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpGet("{shopId}")]
        public async Task<ApiResult<ShopInterfaceDTO>> GetShopInterface(int shopId)
        {
            var command = new FindShopInterfaceByShopIdCommand
            {
                ShopId = shopId
            };

            var result = await _mediator.Send(command);

            return new ApiResult<ShopInterfaceDTO> { ResponseCode = 200, Data = result.Response };
        }
    }
}

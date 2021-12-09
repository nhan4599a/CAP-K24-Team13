using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.DTOs;
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
        protected IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        [HttpGet]
        [Route("/api/shop")]
        public async Task<CommandResponse<ShopInterfaceDTO>> Get(string searchString)
        {
            var result = await Mediator.Send(new FindShop.Query { SearchString = searchString });
            return result;
        }
        [HttpPost]
        [Route("api/shop")]
        public async Task<CommandResponse<ShopInterfaceDTO>> AddOrEditShop(ShopInterfaceDTO model)
        {
            var result = await Mediator.Send(new AddOrEditShop.Command { shopInterfaceDTO = model });
            return result;
        }
    }
}

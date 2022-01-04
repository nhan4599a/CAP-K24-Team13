using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using ShopProductService.Commands.Cart;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ActionName("GetCartItems")]
        public async Task<ApiResult<List<CartItemDto>>> GetCartItems()
        {
            var response = await _mediator.Send(new GetCartItemListQuery { UserId = "69" });
            return new ApiResult<List<CartItemDto>> { ResponseCode = 200, Data = response };
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<ApiResult<bool>> AddCartItem(AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new AddCartItemCommand
            {
                requestModel = requestModel,
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
        [HttpPut]
        [ActionName("EditQuantity")]
        public async Task<ApiResult<bool>> EditQuantity(AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new EditQuantityCartItemCommand
            {
                requestModel = requestModel,
            });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
        [HttpDelete]
        [ActionName("RemoveCartItem")]
        public async Task<ApiResult<bool>> RemoveCartItem(RemoveCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new RemoveCartItemCommand { requestModel = requestModel });
            if (!response.Response)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
    }
}

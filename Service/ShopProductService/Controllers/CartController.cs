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

        [HttpGet("{userId}/items")]
        public async Task<List<CartItemDTO>> GetCartItems(string userId)
        {
            return await _mediator.Send(new GetCartItemsQuery { UserId = userId });
        }

        [HttpPost]
        public async Task<ApiResult<bool>> AddCartItem([FromForm] AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new AddCartItemCommand
            {
                RequestModel = requestModel,
            });
            if (!response.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpPut]
        public async Task<ApiResult<bool>> EditQuantity([FromForm] AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new EditQuantityCartItemCommand
            {
                requestModel = requestModel,
            });
            if (!response.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<ApiResult<bool>> RemoveCartItem(string userId, string productId)
        {
            var response = await _mediator.Send(new RemoveCartItemCommand
            { 
                requestModel = new RemoveCartItemRequestModel
                {
                    UserId = userId,
                    ProductId = productId
                }
            });
            if (!response.IsSuccess)
                return new ApiResult<bool> { ResponseCode = 500, ErrorMessage = response.ErrorMessage, Data = false };
            return new ApiResult<bool> { ResponseCode = 200, Data = true };
        }
    }
}

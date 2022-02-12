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
        public async Task<ApiResult> GetCartItems(string userId)
        {
            var result = await _mediator.Send(new GetCartItemsQuery { UserId = userId });
            return ApiResult<List<CartItemDTO>>.CreateSuccessResult(result);
        }

        [HttpPost]
        public async Task<ApiResult> AddCartItem([FromForm] AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new AddCartItemCommand
            {
                RequestModel = requestModel,
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSuccessResult(true);
        }

        [HttpPut]
        public async Task<ApiResult> EditQuantity([FromForm] AddOrEditQuantityCartItemRequestModel requestModel)
        {
            var response = await _mediator.Send(new EditQuantityCartItemCommand
            {
                requestModel = requestModel,
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSuccessResult(true);
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<ApiResult> RemoveCartItem(string userId, string productId)
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
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<bool>.CreateSuccessResult(true);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHistoryService.Commands;
using OrderService.Commands;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderHistoryService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user/{userId}")]
        public async Task<ApiResult> GetOrderHistory(string userId)
        {
            var result = await _mediator.Send(new GetOrderHistoryByUserIdQuery
            {
                UserId = userId
            });
            return ApiResult<List<OrderItemDTO>>.CreateSucceedResult(result);
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ApiResult> GetOrdersOfShop(int shopId)
        {
            var result = await _mediator.Send(new GetNearByOrdersOfShopQuery
            {
                ShopId = shopId
            });
            return ApiResult<List<OrderDTO>>.CreateSucceedResult(result);
        }

        [HttpPost("{invoiceId}")]
        public async Task<ApiResult> ChangeOrderStatus(int invoiceId, [FromBody] InvoiceStatus newStatus)
        {
            var result = await _mediator.Send(new ChangeOrderStatusCommand
            {
                InvoiceId = invoiceId,
                NewStatus = newStatus
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            return ApiResult<bool>.CreateSucceedResult(result.Response);
        }
    }
}

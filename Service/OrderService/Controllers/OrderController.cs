using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderHistoryService.Commands;
using OrderService;
using OrderService.Commands;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderHistoryService.Controllers
{
    [Authorize]
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
        public async Task<ApiResult> ChangeOrderStatus(int invoiceId, [FromBody] int newStatusInt)
        {
            var result = await _mediator.Send(new ChangeOrderStatusCommand
            {
                InvoiceId = invoiceId,
                NewStatus = (InvoiceStatus)newStatusInt
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            return ApiResult<bool>.CreateSucceedResult(result.Response);
        }

        [HttpGet("search")]
        public async Task<ApiResult> FindOrderById([FromQuery] FindInvoiceQuery query)
        {
            var result = await _mediator.Send(query);
            if (result == null)
                return ApiResult.CreateErrorResult(404, "Invoice not found");
            return ApiResult<PaginatedList<InvoiceDTO>>.CreateSucceedResult(result);
        }
    }
}

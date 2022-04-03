using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using OrderService.Commands;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderService.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IDistributedCache _cache;

        private static readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        public OrderController(IMediator mediator, IDistributedCache cache)
        {
            _mediator = mediator;
            _cache = cache;
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

        [AllowAnonymous]
        [HttpGet("shop/{shopId}/search")]
        public async Task<ApiResult> FindOrders(int shopId, [FromQuery] FindInvoiceQuery query)
        {
            query.ShopId = shopId;
            var response = await _mediator.Send(query);
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult<PaginatedList<InvoiceDTO>>.CreateSucceedResult(response.Response);
        }
        
        [HttpGet("{invoiceCode}")]
        public async Task<ApiResult> GetOrderDetail(string invoiceCode)
        {
            var cachedResult = _cache.GetString(GetCacheKey(invoiceCode));
            if (cachedResult != null)
                return ApiResult<InvoiceDetailDTO>.CreateSucceedResult(JsonSerializer.Deserialize<InvoiceDetailDTO>(cachedResult)!);
            var response = await _mediator.Send(new GetInvoiceByInvoiceCodeQuery
            {
                InvoiceCode = invoiceCode 
            });
            if (response == null)
                return ApiResult.CreateErrorResult(404, "Invoice not found");
            await _cache.SetStringAsync(GetCacheKey(invoiceCode), JsonSerializer.Serialize(response), cacheOptions);
            if (User.FindFirstValue("ShopId") != response.ShopId.ToString())
                return ApiResult.CreateErrorResult(403, "User does not have permission to view order detail");
            return ApiResult<InvoiceDetailDTO>.CreateSucceedResult(response);
        }

        private static string GetCacheKey(string invoiceCode)
        {
            return $"Order.{invoiceCode}";
        }
    }
}

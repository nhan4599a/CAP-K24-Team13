using IdentityModel.Client;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OrderService.Commands;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
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

        private readonly IConfiguration _configuration;

        private static readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        public OrderController(IMediator mediator, IDistributedCache cache, IConfiguration configuration)
        {
            _mediator = mediator;
            _cache = cache;
            _configuration = configuration;
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
                return ApiResult<InvoiceDetailDTO>.CreateSucceedResult(System.Text.Json.JsonSerializer.Deserialize<InvoiceDetailDTO>(cachedResult)!);
            var response = await _mediator.Send(new GetInvoiceByInvoiceCodeQuery
            {
                InvoiceCode = invoiceCode 
            });
            if (response == null)
                return ApiResult.CreateErrorResult(404, "Invoice not found");
            await _cache.SetStringAsync(GetCacheKey(invoiceCode), System.Text.Json.JsonSerializer.Serialize(response), cacheOptions);
            if (User.FindFirstValue("ShopId") != response.ShopId.ToString())
                return ApiResult.CreateErrorResult(403, "User does not have permission to view order detail");
            return ApiResult<InvoiceDetailDTO>.CreateSucceedResult(response);
        }
        
        [HttpGet("ref/{refId}")]
        public async Task<ApiResult> GetOrderDetailByRefIf(string refId)
        {
            var cachedResult = _cache.GetString(GetCacheKey(refId, true));
            if (cachedResult != null)
                return ApiResult<InvoiceDetailDTO[]>.CreateSucceedResult(System.Text.Json.JsonSerializer.Deserialize<InvoiceDetailDTO[]>(cachedResult)!);
            var response = await _mediator.Send(new FindInvoiceByRefIdQuery
            {
                RefId = refId
            });
            await _cache.SetStringAsync(GetCacheKey(refId, true), System.Text.Json.JsonSerializer.Serialize(response), cacheOptions);
            return ApiResult<InvoiceDetailDTO[]>.CreateSucceedResult(response);
        }

        [HttpPost("paid/{refId}")]
        [AllowAnonymous]
        public async Task<ApiResult> MakeAsPaid(string refId, MakeAsPaidRequestModel requestModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var validateResult = await jwtTokenHandler.ValidateTokenAsync(requestModel.AccessToken, new TokenValidationParameters
            {
                ValidateAudience = false
            });
            if (!validateResult.IsValid)
                return ApiResult.CreateErrorResult(401, "Authorized token");
            byte[] keyBytes = Encoding.UTF8.GetBytes(_configuration["MOMO_SECRET_KEY"]);
            var ipnRequest = JsonConvert.DeserializeObject<MomoWalletIpnRequest>(requestModel.WalletIpnRequest)!;
            ipnRequest.AccessKey = _configuration["MOMO_ACCESS_KEY"];
            byte[] messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(ipnRequest));
            using var hmacsha256 = new HMACSHA256(keyBytes);
            byte[] hashedBytes = hmacsha256.ComputeHash(messageBytes);
            var hashedMessage = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            if (hashedMessage != ipnRequest.Signature)
                return ApiResult.CreateErrorResult(400, "Invalid momo ipn request");
            var response = await _mediator.Send(new MakeAsPaidInvoiceCommand
            {
                RefId = refId
            });
            return ApiResult.SucceedResult;
        }

        private static string GetCacheKey(string value, bool isRefId = false)
        {
            if (!isRefId)
                return $"Order.{value}";
            return $"Order.Ref.{value}";
        }
    }
}

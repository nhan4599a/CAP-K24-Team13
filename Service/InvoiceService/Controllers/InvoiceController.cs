using InvoiceService.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SystemJson = System.Text.Json;

namespace OrderService.Controllers
{
    [Authorize]
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IMediator _mediator;

        private readonly IDistributedCache _cache;

        private readonly IConfiguration _configuration;

        private static readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        public InvoiceController(IMediator mediator, IDistributedCache cache, IConfiguration configuration)
        {
            _mediator = mediator;
            _cache = cache;
            _configuration = configuration;
        }

        [HttpGet("user/{userId}")]
        public async Task<ApiResult> GetOrderHistory(string userId)
        {
            var result = await _mediator.Send(new GetOrderHistoryQuery
            {
                UserId = userId
            });
            return ApiResult<Dictionary<string, InvoiceWithItemDTO[]>>.CreateSucceedResult(result);
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ApiResult> GetNearByInvoicesOfShop(int shopId)
        {
            var result = await _mediator.Send(new GetNearByInvoicesOfShopQuery
            {
                ShopId = shopId
            });
            return ApiResult<List<InvoiceDTO>>.CreateSucceedResult(result);
        }

        [HttpPost("{invoiceId}")]
        public async Task<ApiResult> ChangeOrderStatus(int invoiceId, [FromBody] int newStatusInt)
        {
            var result = await _mediator.Send(new ChangeInvoiceStatusCommand
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
            return ApiResult<PaginatedList<InvoiceWithReportDTO>>.CreateSucceedResult(response.Response);
        }
        
        [HttpGet("{invoiceCode}")]
        public async Task<ApiResult> GetInvoiceDetail(string invoiceCode)
        {
            var cachedResult = _cache.GetString(GetCacheKey(invoiceCode));
            if (cachedResult != null)
                return ApiResult<InvoiceWithItemDTO>.CreateSucceedResult(SystemJson.JsonSerializer.Deserialize<InvoiceWithItemDTO>(cachedResult)!);
            var response = await _mediator.Send(new GetInvoiceByInvoiceCodeQuery
            {
                InvoiceCode = invoiceCode
            });
            if (response == null)
                return ApiResult.CreateErrorResult(404, "Invoice not found");
            await _cache.SetStringAsync(GetCacheKey(invoiceCode), SystemJson.JsonSerializer.Serialize(response), cacheOptions);
            if (User.FindFirstValue("ShopId") != response.ShopId.ToString())
                return ApiResult.CreateErrorResult(403, "User does not have permission to view order detail");
            return ApiResult<InvoiceWithItemDTO>.CreateSucceedResult(response);
        }
        
        [HttpGet("ref/{refId}")]
        public async Task<ApiResult> GetOrderDetailByRefIf(string refId)
        {
            var cachedResult = _cache.GetString(GetCacheKey(refId, true));
            if (cachedResult != null)
                return ApiResult<InvoiceWithItemDTO[]>.CreateSucceedResult(SystemJson.JsonSerializer.Deserialize<InvoiceWithItemDTO[]>(cachedResult)!);
            var response = await _mediator.Send(new FindInvoicesByRefIdQuery
            {
                RefId = refId
            });
            await _cache.SetStringAsync(GetCacheKey(refId, true), SystemJson.JsonSerializer.Serialize(response), cacheOptions);
            return ApiResult<InvoiceWithItemDTO[]>.CreateSucceedResult(response);
        }

        [HttpPost("post-payment/{refId}")]
        public async Task<ApiResult> PostPaymentProcessing(string refId, AfterPaymentProcessingRequest requestModel)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(_configuration["MOMO_SECRET_KEY"]);
            var accessToken = await HttpContext.GetTokenAsync(Shared.SystemConstant.Authentication.ACCESS_TOKEN_KEY);
            if (accessToken != requestModel.AccessToken)
                return ApiResult.CreateErrorResult(401, "Unauthorized token");
            var ipnRequest = JsonConvert.DeserializeObject<MomoWalletIpnRequest>(requestModel.WalletIpnRequest)!;
            ipnRequest.AccessKey = _configuration["MOMO_ACCESS_KEY"];
            byte[] messageBytes = Encoding.UTF8.GetBytes(ipnRequest.GetSecurityMessage());
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

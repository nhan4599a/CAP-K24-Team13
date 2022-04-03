using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models;
using StatisticService.Commands;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace StatisticService.Controllers
{
    [ApiController]
    [Route("/api/statistic")]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IDistributedCache _cache;

        private static readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };

        public StatisticController(IMediator mediator, IDistributedCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet("shop/{shopId}/orders")]
        public async Task<ApiResult> StatisticOrder(int shopId,
            [FromQuery(Name = "strategy")] StatisticStrategy strategy,
            [FromQuery(Name = "start")] string start,
            [FromQuery(Name = "end")] string end)
        {
            var parseResult = StatisticDateRange.TryCreate(strategy, start, end, out StatisticDateRange range);
            if (!parseResult.IsSucceed)
                return ApiResult.CreateErrorResult(400, parseResult.Exception.Message);
            var cachedResult = _cache.GetString(GetCacheKey(strategy, range));
            if (cachedResult != null)
            {
                return ApiResult<StatisticResult>.CreateSucceedResult(JsonSerializer.Deserialize<StatisticResult>(cachedResult)!);
            }
            try
            {
                var result = await _mediator.Send(new OrderStatisticCommand
                {
                    ShopId = shopId,
                    Strategy = strategy,
                    Range = range
                });
                await _cache.SetStringAsync(GetCacheKey(strategy, range), JsonSerializer.Serialize(result), cacheOptions);
                return ApiResult<StatisticResult>.CreateSucceedResult(result);
            }
            catch (Exception e)
            {
                return ApiResult.CreateErrorResult(500, e.Message);
            }
        }

        private static string GetCacheKey(StatisticStrategy strategy, StatisticDateRange range)
        {
            return $"Statistic.{strategy}.{range.Range.Start:dd/MM/yyyy}-{range.Range.End:dd/MM/yyyy}";
        }
    }
}

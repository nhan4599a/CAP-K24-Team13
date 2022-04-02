using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using StatisticService.Commands;
using System;
using System.Threading.Tasks;

namespace StatisticService.Controllers
{
    [ApiController]
    [Route("/api/statistic")]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticController(IMediator mediator)
        {
            _mediator = mediator;
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
            try
            {
                var result = await _mediator.Send(new OrderStatisticCommand
                {
                    ShopId = shopId,
                    Strategy = strategy,
                    Range = range
                });
                return ApiResult<StatisticResult>.CreateSucceedResult(result);
            }
            catch (Exception e)
            {
                return ApiResult.CreateErrorResult(500, e.Message);
            }
        }
    }
}

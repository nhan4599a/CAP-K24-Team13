using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using StatisticService.Commands;
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
            [FromQuery(Name = "strategy")] StatisticStrategy strategy = StatisticStrategy.ByMonth)
        {
            var result = await _mediator.Send(new OrderStatisticCommand
            {
                ShopId = shopId,
                Strategy = strategy
            });
            return ApiResult<StatisticResult>.CreateSucceedResult(result);
        }
    }
}

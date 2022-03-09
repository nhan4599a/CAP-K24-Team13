using DatabaseAccessor.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using StatisticService.Commands;
using System.Threading.Tasks;

namespace StatisticService.Controllers
{
    [ApiController]
    [Route("/api/statistic")]
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("orders")]
        public async Task<ApiResult> StatisticOrder(
            [FromQuery(Name = "strategy")] StatisticStrategy strategy = StatisticStrategy.ByMonth)
        {
            var result = await _mediator.Send(new OrderStatisticCommand
            {
                Strategy = strategy
            });
            return ApiResult<StatisticResult>.CreateSucceedResult(result);
        }
    }
}

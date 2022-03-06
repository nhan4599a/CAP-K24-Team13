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
        public async Task<ApiResult> StatisticOrder(StatisticStrategy strategy, OrderStatusField field)
        {
            var result = await _mediator.Send(new OrderStatisticCommand
            {
                Strategy = strategy,
                Field = field
            });
            return ApiResult<StatisticResult>.CreateSucceedResult(result);
        }
    }
}

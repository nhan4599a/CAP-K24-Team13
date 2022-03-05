using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

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
        public async ApiResult StatisticOrder(StatisticStrategy strategy, OrderStatusField field)
        {

        }
    }
}

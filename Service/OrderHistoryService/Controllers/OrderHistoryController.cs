using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderHistoryService.Commands;
using Shared.DTOs;
using Shared.Models;

namespace OrderHistoryService.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderHistoryController : Controller
    {
        private readonly IMediator _mediator;

        public OrderHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<ApiResult<List<OrderUserHistoryDTO>>> GetOrderHistory(string userId)
        {
            var result = await _mediator.Send(new GetOrderHistoryQuery
            {
                UserId = userId
            });
            return new ApiResult<List<OrderUserHistoryDTO>> { ResponseCode = 200, Data = result };
        }
    }
}

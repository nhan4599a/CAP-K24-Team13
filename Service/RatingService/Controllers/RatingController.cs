using MediatR;
using Microsoft.AspNetCore.Mvc;
using RatingService.Command;
using Shared.DTOs;
using Shared.Models;

namespace RatingService.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingController : Controller
    {
        private readonly IMediator _mediator;
        public RatingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{productId}")]
        public async Task<ApiResult<List<RatingDTO>>> GetRating(string productId)
        {
            var result = await _mediator.Send(new GetRatingQuery
            {
                ProductId = productId

            });
            return new ApiResult<List<RatingDTO>> { ResponseCode = 200, Data = result };
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using RatingService.Command;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;

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

        [HttpPost]
        public async Task<ApiResult> RatingProduct
            ([FromForm(Name = "requestModel")] RatingRequestModel requestModel)
        {
            var result = await _mediator.Send(new RatingProductCommand
            {
                RequestModel = requestModel
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            return ApiResult.SucceedResult;
        }
    }
}

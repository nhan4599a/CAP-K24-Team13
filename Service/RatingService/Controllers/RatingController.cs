using MediatR;
using Microsoft.AspNetCore.Mvc;
using RatingService.Commands;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RatingService.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RatingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{productId}")]
        public async Task<ApiResult<List<RatingDTO>>> GetRating(string productId)
        {
            var result = await _mediator.Send(new GetRatingByProductIdQuery
            {
                ProductId = productId
            });
            return ApiResult<List<RatingDTO>>.CreateSucceedResult(result);
        }

        [HttpPost]
        public async Task<ApiResult> RatingProduct
            ([FromForm] RatingRequestModel requestModel)
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

using MediatR;
using Shared.DTOs;

namespace RatingService.Commands
{
    public class GetRatingByProductIdQuery : IRequest<List<RatingDTO>>
    {
        public string ProductId { get; set; }
    }
}

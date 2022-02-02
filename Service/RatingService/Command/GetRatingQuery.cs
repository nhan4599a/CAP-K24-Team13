using MediatR;
using Shared.DTOs;

namespace RatingService.Command
{
    public class GetRatingQuery : IRequest<List<RatingDTO>>
    {
        public string ProductId { get; set; }
    }
}

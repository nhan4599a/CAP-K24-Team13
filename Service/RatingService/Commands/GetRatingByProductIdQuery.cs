using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace RatingService.Commands
{
    public class GetRatingByProductIdQuery : IRequest<List<RatingDTO>>
    {
        public string? ProductId { get; set; }
    }
}

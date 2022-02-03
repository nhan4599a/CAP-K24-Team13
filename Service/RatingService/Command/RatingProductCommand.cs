using MediatR;
using Shared;
using Shared.RequestModels;

namespace RatingService.Command
{
    public class RatingProductCommand : IRequest<CommandResponse<bool>>
    {
        public RatingRequestModel RequestModel { get; set; }
    }
}

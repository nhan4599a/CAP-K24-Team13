using MediatR;
using Shared.Models;
using Shared.RequestModels;

namespace RatingService.Commands
{
    public class RatingProductCommand : IRequest<CommandResponse<bool>>
    {
        public RatingRequestModel? RequestModel { get; set; }
    }
}

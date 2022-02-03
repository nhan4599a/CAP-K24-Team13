using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using RatingService.Command;
using Shared;
using Shared.DTOs;

namespace RatingService.Handler
{


    public class RatingProductCommandHandler : IRequestHandler<RatingProductCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingProductCommandHandler(IRatingRepository RatingRepository)
        {
            _ratingRepository = RatingRepository;
        }


        public void Dispose()
        {
            _ratingRepository.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<CommandResponse<bool>> Handle(RatingProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _ratingRepository.RatingProductAsync(request.RequestModel);
            return result;
        }
    }
}

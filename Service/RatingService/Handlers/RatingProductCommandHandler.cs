using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using RatingService.Commands;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RatingService.Handlers
{
    public class RatingProductCommandHandler : IRequestHandler<RatingProductCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingProductCommandHandler(IRatingRepository RatingRepository)
        {
            _ratingRepository = RatingRepository;
        }

        public async Task<CommandResponse<bool>> Handle(RatingProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _ratingRepository.RatingProductAsync(request.RequestModel);
            return result;
        }

        public void Dispose()
        {
            _ratingRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

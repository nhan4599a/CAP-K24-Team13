using MediatR;
using RatingService.Command;
using Shared.DTOs;
using DatabaseAccessor.Repositories.Abstraction;

namespace RatingService.Handler
{
    public class GetRatingCommandHandler : IRequestHandler<GetRatingQuery, List<RatingDTO>>, IDisposable
    {
        private readonly IRatingRepository _ratingRepository;

        public GetRatingCommandHandler(IRatingRepository RatingRepository)
        {
            _ratingRepository = RatingRepository;
        }


        public void Dispose()
        {
            _ratingRepository.Dispose();
            GC.SuppressFinalize(this);
        }


        public async Task<List<RatingDTO>> Handle(GetRatingQuery request, CancellationToken cancellationToken)
        {
            return await _ratingRepository.GetRatingAsync(request.ProductId);
        }
    }
}

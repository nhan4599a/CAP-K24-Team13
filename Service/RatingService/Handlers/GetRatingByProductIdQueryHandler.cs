using MediatR;
using RatingService.Commands;
using Shared.DTOs;
using DatabaseAccessor.Repositories.Abstraction;

namespace RatingService.Handlers
{
    public class GetRatingByProductIdQueryHandler : IRequestHandler<GetRatingByProductIdQuery, List<RatingDTO>>, IDisposable
    {
        private readonly IRatingRepository _ratingRepository;

        public GetRatingByProductIdQueryHandler(IRatingRepository RatingRepository)
        {
            _ratingRepository = RatingRepository;
        }


        public void Dispose()
        {
            _ratingRepository.Dispose();
            GC.SuppressFinalize(this);
        }


        public async Task<List<RatingDTO>> Handle(GetRatingByProductIdQuery request, CancellationToken cancellationToken)
        {
            return await _ratingRepository.GetRatingAsync(request.ProductId);
        }
    }
}

using DatabaseAccessor.Contexts;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IRatingRepository : IEFCoreRepository<ApplicationDbContext>
    {
        Task<List<RatingDTO>> GetRatingAsync(string productId);
        Task<CommandResponse<bool>> RatingProductAsync(RatingRequestModel rating);
    }
}

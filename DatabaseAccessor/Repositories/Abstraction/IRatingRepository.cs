using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IRatingRepository : IDisposable 
    {
        Task<List<RatingDTO>> GetRatingAsync(string productId);
    }
}

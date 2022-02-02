using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public RatingRepository(ApplicationDbContext context, Mapper mapper)
        {
            _dbContext = context;
            _mapper = mapper
                      ?? Mapper.GetInstance();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
        
        public async Task<List<RatingDTO>> GetRatingAsync(string productId)
        {
            var ratings = await _dbContext.ProductComments.Where(item => item.ProductId.ToString() == productId).ToListAsync();
            return ratings.Select(item => _mapper.MapToRatingDTO(item)).ToList();
        }
    }
}

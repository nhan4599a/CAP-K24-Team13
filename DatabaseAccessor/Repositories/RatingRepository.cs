using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<CommandResponse<bool>> RatingProductAsync(RatingRequestModel requestModel)
        {
            var user = await _dbContext.Users.FindAsync(Guid.Parse(requestModel.UserId));
            if (user == null)
                return CommandResponse<bool>.Error("Invalid User", null);
            var product = await _dbContext.ShopProducts.FindAsync(Guid.Parse(requestModel.ProductId));
            if (product == null)
                return CommandResponse<bool>.Error("Invalid Product", null);
            var bought = await _dbContext.InvoiceDetails
                .AnyAsync(detail => detail.ProductId.ToString() == requestModel.ProductId 
                    && detail.Invoice.UserId.ToString() == requestModel.UserId);
            if (!bought)
                return CommandResponse<bool>.Error("You are not buying", null);
            _dbContext.ProductComments.Add(new ProductComment
            {
                UserId = Guid.Parse(requestModel.UserId),
                ProductId = Guid.Parse(requestModel.ProductId),
                Star = requestModel.Star,
                Message = requestModel.Message
            });
            return CommandResponse<bool>.Success(true);
        }

    }
}

using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
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
            _mapper = mapper ?? Mapper.GetInstance();
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
            var productId = Guid.Parse(requestModel.ProductId);
            if (user == null)
                return CommandResponse<bool>.Error("Invalid User", null);
            var product = await _dbContext.ShopProducts.FindAsync(productId);
            if (product == null)
                return CommandResponse<bool>.Error("Invalid Product", null);
            var buyCount = await _dbContext.InvoiceDetails
                .CountAsync(detail => detail.ProductId.ToString() == requestModel.ProductId 
                    && detail.Invoice.UserId == user.Id
                    && detail.Invoice.Status == InvoiceStatus.Succeed);
            var ratingCount = await _dbContext.ProductComments
                .CountAsync(comment => comment.UserId == user.Id
                    && comment.ProductId == productId);
            if (buyCount - ratingCount <= 0)
                return CommandResponse<bool>.Error("User have not bought yet", null);
            _dbContext.ProductComments.Add(new ProductComment
            {
                UserId = Guid.Parse(requestModel.UserId),
                ProductId = Guid.Parse(requestModel.ProductId),
                Star = requestModel.Star,
                Message = requestModel.Message
            });
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

    }
}

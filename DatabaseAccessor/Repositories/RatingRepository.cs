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
            return await _dbContext.ProductComments
                .AsNoTracking()
                .Where(item => item.ProductId == Guid.Parse(productId))
                .Select(item => _mapper.MapToRatingDTO(item))
                .ToListAsync();
        }

        public async Task<CommandResponse<bool>> RatingProductAsync(RatingRequestModel requestModel)
        {
            var productId = Guid.Parse(requestModel.ProductId);
            var invoice = await _dbContext.InvoiceDetails
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(item => item.InvoiceId == requestModel.InvoiceId && item.ProductId == productId);
            if (invoice == null)
                return CommandResponse<bool>.Error("Invalid rating action", null);
            if (invoice.Invoice.Status != InvoiceStatus.Succeed)
                return CommandResponse<bool>.Error("Invoice must be succeed to rating", null);
            if (invoice.IsCommented)
                return CommandResponse<bool>.Error("You are already rating this product", null);
            _dbContext.ProductComments.Add(new ProductComment
            {
                UserId = invoice.Invoice.UserId,
                ProductId = productId,
                Star = requestModel.Star,
                Message = requestModel.Message
            });
            invoice.IsCommented = true;
            await _dbContext.SaveChangesAsync();
            return CommandResponse<bool>.Success(true);
        }

    }
}

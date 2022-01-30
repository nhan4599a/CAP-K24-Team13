using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderHistoryService.Repository
{

    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public OrderHistoryRepository(ApplicationDbContext context, Mapper mapper)
        {
            _dbContext = context;
            _mapper = mapper ?? Mapper.GetInstance();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<List<OrderUserHistoryDTO>> GetOrderHistoryAsync(string userId)
        {
            var invoice = await _dbContext.Invoices.FirstOrDefaultAsync(invoice => invoice.UserId.ToString() == userId);
            if (invoice == null)
                return new List<OrderUserHistoryDTO>();
            return invoice.Details.Select(item => _mapper.MapToOrderUserHistoryDTO(item)).ToList();
        }
    }
}


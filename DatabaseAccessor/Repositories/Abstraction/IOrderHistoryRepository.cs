using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderHistoryService.Repository
{
    
        public interface IOrderHistoryRepository : IDisposable
        {

            Task<List<OrderUserHistoryDTO>> GetOrderHistoryAsync(string userId);
        }
    
}

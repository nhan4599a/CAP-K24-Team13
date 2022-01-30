using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    
        public interface IOrderHistoryRepository : IDisposable
        {

            Task<List<OrderUserHistoryDTO>> GetOrderHistoryAsync(string userId);
        }
    
}

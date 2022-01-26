using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IOrderRepository : IDisposable
    {
        Task<CommandResponse<bool>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingAddress);
    }
}

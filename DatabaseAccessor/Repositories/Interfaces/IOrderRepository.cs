using Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<CommandResponse<bool>> AddOrderAsync(List<Guid> productIds);
    }
}

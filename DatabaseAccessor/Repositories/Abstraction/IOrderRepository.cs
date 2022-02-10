using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IOrderRepository : IDisposable
    {
        Task<CommandResponse<bool>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingName,
            string shippingPhone, string shippingAddress, string orderNotes);

        Task<CommandResponse<List<InvoiceDTO>>> GetOrderListAsync(GetInvoiceListRequestModel requestModel);

        Task<CommandResponse<InvoiceDTO>> GetOrderDetailAsync(int invoiceId);
    }
}

using Shared;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IInvoiceRepository : IDisposable
    {

        Task<List<OrderItemDTO>> GetOrderHistoryAsync(string userId);

        Task<List<OrderDTO>> GetOrdersOfShopWithInTimeAsync(int shopId, DateOnly startDate, DateOnly endDate);

        Task<List<OrderDTO>> GetOrdersOfShopAsync(int shopId);

        Task<CommandResponse<bool>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingName,
            string shippingPhone, string shippingAddress, string orderNotes);

        Task<CommandResponse<bool>> ChangeOrderStatusAsync(int invoiceId, InvoiceStatus newStatus);
    }

}

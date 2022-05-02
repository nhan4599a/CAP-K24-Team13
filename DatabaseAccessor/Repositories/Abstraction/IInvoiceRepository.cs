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

        Task<CommandResponse<string>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingName,
            string shippingPhone, string shippingAddress, string orderNotes, PaymentMethod paymentMethod);

        Task<CommandResponse<bool>> ChangeOrderStatusAsync(int invoiceId, InvoiceStatus newStatus);

        Task<CommandResponse<PaginatedList<InvoiceDTO>>> FindInvoicesAsync(int shopId, string key, string value,
            PaginationInfo paginationInfo);

        Task<InvoiceDetailDTO> GetInvoiceDetailAsync(string invoiceCode);

        Task<InvoiceDetailDTO[]> GetInvoiceDetailByRefIdAsync(string refId);

        Task MakeAsPaidAsync(string refId);

        Task<StatisticResult> StatisticAsync(int shopId, StatisticStrategy strategy, StatisticDateRange range);
    }
}

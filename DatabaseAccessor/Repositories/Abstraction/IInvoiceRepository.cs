using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories.Abstraction
{
    public interface IInvoiceRepository : IDisposable
    {
        Task<Dictionary<string, InvoiceWithItemDTO[]>> GetOrderHistoryAsync(string userId);

        Task<List<InvoiceDTO>> GetOrdersOfShopWithInTimeAsync(int shopId, DateOnly startDate, DateOnly endDate);

        Task<CommandResponse<string>> AddOrderAsync(Guid userId, List<Guid> productIds, string shippingName,
            string shippingPhone, string shippingAddress, string orderNotes, PaymentMethod paymentMethod);

        Task<CommandResponse<bool>> ChangeOrderStatusAsync(int invoiceId, InvoiceStatus newStatus);

        Task<CommandResponse<PaginatedList<InvoiceWithReportDTO>>> FindInvoicesAsync(int shopId, string key, string value,
            PaginationInfo paginationInfo);

        Task<FullInvoiceDTO> GetInvoiceDetailAsync(string invoiceCode);

        Task<InvoiceWithItemDTO[]> GetInvoiceDetailByRefIdAsync(string refId);

        Task MakeAsPaidAsync(string refId);

        Task RemoveInvoiceAsync(string refId);

        Task<StatisticResult> StatisticAsync(int shopId, StatisticStrategy strategy, StatisticDateRange range);
    }
}

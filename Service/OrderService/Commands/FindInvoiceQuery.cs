using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace OrderService.Commands
{
    public record class FindInvoiceQuery(string Key, object Value)
        : IRequest<PaginatedList<InvoiceDTO>>
    {
        public int ShopId { get; set; }

        public int PageNumber { get; set; } = PaginationInfo.Default.PageNumber;

        public int PageSize { get; set; } = PaginationInfo.Default.PageSize;
    }
}

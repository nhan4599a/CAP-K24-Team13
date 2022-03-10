using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace OrderService.Commands
{
    public class FindInvoiceQuery : IRequest<PaginatedList<InvoiceDTO>>
    {
        public int ShopId { get; set; }

        public string? Key { get; set; }

        public object? Value { get; set; }

        public int PageNumber { get; set; } = PaginationInfo.Default.PageNumber;

        public int PageSize { get; set; } = PaginationInfo.Default.PageSize;
    }
}

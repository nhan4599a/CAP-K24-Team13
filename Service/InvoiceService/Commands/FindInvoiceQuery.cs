using MediatR;
using Shared.DTOs;
using Shared.Models;

namespace InvoiceService.Commands
{
    public class FindInvoiceQuery : IRequest<CommandResponse<PaginatedList<InvoiceWithReportDTO>>>
    {
        public int ShopId { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }

        public int PageNumber { get; set; } = PaginationInfo.Default.PageNumber;

        public int PageSize { get; set; } = PaginationInfo.Default.PageSize;
    }
}

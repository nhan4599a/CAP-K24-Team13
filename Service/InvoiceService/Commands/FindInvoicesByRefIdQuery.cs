using MediatR;
using Shared.DTOs;

namespace InvoiceService.Commands
{
    public class FindInvoicesByRefIdQuery : IRequest<InvoiceWithItemDTO[]>
    {
        public string? RefId { get; set; }
    }
}

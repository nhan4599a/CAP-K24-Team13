using MediatR;
using Shared.DTOs;

namespace InvoiceService.Commands
{
    public class GetInvoiceByInvoiceCodeQuery : IRequest<InvoiceWithItemDTO>
    {
        public string? InvoiceCode { get; set; }
    }
}

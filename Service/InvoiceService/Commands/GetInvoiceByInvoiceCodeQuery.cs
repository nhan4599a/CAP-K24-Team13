using MediatR;
using Shared.DTOs;

namespace InvoiceService.Commands
{
    public class GetInvoiceByInvoiceCodeQuery : IRequest<FullInvoiceDTO>
    {
        public string? InvoiceCode { get; set; }
    }
}

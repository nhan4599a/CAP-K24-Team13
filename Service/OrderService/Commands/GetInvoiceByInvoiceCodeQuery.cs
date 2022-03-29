using MediatR;
using Shared.DTOs;

namespace OrderService.Commands
{
    public class GetInvoiceByInvoiceCodeQuery : IRequest<InvoiceDetailDTO>
    {
        public string? InvoiceCode { get; set; }
    }
}

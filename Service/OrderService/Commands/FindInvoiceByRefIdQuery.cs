using MediatR;
using Shared.DTOs;

namespace OrderService.Commands
{
    public class FindInvoiceByRefIdQuery : IRequest<InvoiceDetailDTO[]>
    {
        public string? RefId { get; set; }
    }
}

using MediatR;
using Shared;
using Shared.DTOs;

namespace OrderService.Commands
{
    public class GetOrderDetailQuery : IRequest<CommandResponse<InvoiceDTO>>
    {
        public int OrderId { get; set; }
    }
}

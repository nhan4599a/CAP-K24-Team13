using MediatR;
using Shared;
using Shared.Models;

namespace OrderService.Commands
{
    public class ChangeOrderStatusCommand : IRequest<CommandResponse<bool>>
    {
        public int InvoiceId { get; set; }

        public InvoiceStatus NewStatus { get; set; }
    }
}

using MediatR;
using Shared.Models;

namespace InvoiceService.Commands
{
    public class ChangeInvoiceStatusCommand : IRequest<CommandResponse<bool>>
    {
        public int InvoiceId { get; set; }

        public InvoiceStatus NewStatus { get; set; }
    }
}

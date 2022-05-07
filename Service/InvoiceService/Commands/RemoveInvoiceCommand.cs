using MediatR;

namespace InvoiceService.Commands
{
    public class RemoveInvoiceCommand : IRequest
    {
        public string? RefId { get; set; }
    }
}

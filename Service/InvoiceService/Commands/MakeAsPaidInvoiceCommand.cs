using MediatR;

namespace InvoiceService.Commands
{
    public class MakeAsPaidInvoiceCommand : IRequest
    {
        public string? RefId { get; set; }
    }
}

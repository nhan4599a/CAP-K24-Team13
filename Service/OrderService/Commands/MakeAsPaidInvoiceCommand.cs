using MediatR;
using Shared.Models;

namespace OrderService.Commands
{
    public class MakeAsPaidInvoiceCommand : IRequest
    {
        public string? RefId { get; set; }
    }
}

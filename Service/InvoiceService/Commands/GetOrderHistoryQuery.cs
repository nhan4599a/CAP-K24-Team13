using MediatR;
using Shared.DTOs;

namespace InvoiceService.Commands
{
    public class GetOrderHistoryQuery : IRequest<InvoiceWithItemDTO[]>
    {
        public string? UserId { get; set; }  
    }
}

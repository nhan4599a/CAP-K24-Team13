using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace InvoiceService.Commands
{
    public class GetOrderHistoryQuery : IRequest<Dictionary<string, InvoiceWithItemDTO[]>>
    {
        public string? UserId { get; set; }  
    }
}

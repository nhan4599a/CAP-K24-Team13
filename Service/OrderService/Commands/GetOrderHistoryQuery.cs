using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace OrderHistoryService.Commands
{
    public class GetOrderHistoryQuery: IRequest<List<OrderUserHistoryDTO>>
    {
        public string UserId { get; set; }  
    }
}

using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace OrderHistoryService.Commands
{
    public class GetOrderHistoryByUserIdQuery: IRequest<List<OrderDTO>>
    {
        public string UserId { get; set; }  
    }
}

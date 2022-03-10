using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace OrderService.Commands
{
    public class GetOrderHistoryByUserIdQuery : IRequest<List<OrderItemDTO>>
    {
        public string? UserId { get; set; }  
    }
}

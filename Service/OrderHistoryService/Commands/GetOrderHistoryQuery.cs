using MediatR;
using Shared.DTOs;

namespace OrderHistoryService.Commands
{
    public class GetOrderHistoryQuery: IRequest<List<OrderUserHistoryDTO>>
    {
        public string UserId { get; set; }  
    }
}

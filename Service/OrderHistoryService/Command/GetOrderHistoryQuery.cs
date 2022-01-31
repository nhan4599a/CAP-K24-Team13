using MediatR;
using Shared.DTOs;

namespace OrderHistoryService.Command
{
    public class GetOrderHistoryQuery: IRequest<List<OrderUserHistoryDTO>>
    {
        public string UserId { get; set; }  
    }
}

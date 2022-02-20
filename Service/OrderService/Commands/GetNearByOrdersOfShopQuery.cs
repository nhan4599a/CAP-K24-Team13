using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace OrderService.Commands
{
    public class GetNearByOrdersOfShopQuery : IRequest<List<OrderDTO>>
    {
        public int ShopId { get; set; }
    }
}

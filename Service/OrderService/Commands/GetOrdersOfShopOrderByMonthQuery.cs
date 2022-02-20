using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace OrderService.Commands
{
    public class GetOrdersOfShopOrderByMonthQuery : IRequest<List<OrderDTO>>
    {
        public int ShopId { get; set; }
    }
}

using MediatR;
using Shared.DTOs;
using System.Collections.Generic;

namespace OrderService.Commands
{
    public class GetCanceledOrdersOfShopOrderByMonthQuery : IRequest<List<OrderDTO>>
    {
        public int ShopId { get; set; }
    }
}

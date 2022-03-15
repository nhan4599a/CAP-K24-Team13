using MediatR;
using Shared.Models;

namespace StatisticService.Commands
{
    public class OrderStatisticCommand : IRequest<StatisticResult>
    {
        public int ShopId { get; set; }

        public StatisticStrategy Strategy { get; set; }

        public StatisticDateRange? Range { get; set; }
    }
}

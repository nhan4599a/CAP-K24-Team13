using MediatR;
using Shared.Models;

namespace StatisticService.Commands
{
    public class OrderStatisticCommand : IRequest<StatisticResult>
    {
        public StatisticStrategy Strategy { get; set; }
    }
}

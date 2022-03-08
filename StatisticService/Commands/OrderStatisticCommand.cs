using DatabaseAccessor.Models;
using MediatR;
using Shared.Models;

namespace StatisticService.Commands
{
    public class OrderStatisticCommand : IRequest<StatisticResult<Invoice>>
    {
        public StatisticStrategy Strategy { get; set; }
    }
}

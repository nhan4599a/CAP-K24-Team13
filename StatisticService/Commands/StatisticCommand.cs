using MediatR;
using Shared.Models;

namespace StatisticService.Commands
{
    public abstract class StatisticCommand : IRequest<StatisticResult>
    {
        public StatisticStrategy Strategy { get; set; }
    }
}

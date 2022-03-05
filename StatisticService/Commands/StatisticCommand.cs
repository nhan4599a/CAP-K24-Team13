using MediatR;

namespace StatisticService.Commands
{
    public abstract class StatisticCommand : IRequest
    {
        public StatisticStrategy Strategy { get; set; }
    }
}

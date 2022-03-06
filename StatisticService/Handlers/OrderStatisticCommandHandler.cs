using MediatR;
using Shared.Models;
using StatisticService.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace StatisticService.Handlers
{
    public class OrderStatisticCommandHandler : IRequestHandler<OrderStatisticCommand, StatisticResult>
    {
        public Task<StatisticResult> Handle(OrderStatisticCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}

using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using StatisticService.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StatisticService.Handlers
{
    public class OrderStatisticCommandHandler : IRequestHandler<OrderStatisticCommand, StatisticResult>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public OrderStatisticCommandHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public Task<StatisticResult> Handle(OrderStatisticCommand request, CancellationToken cancellationToken)
        {
            return _repository.StatisticAsync(request.ShopId, request.Strategy, request.Range);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

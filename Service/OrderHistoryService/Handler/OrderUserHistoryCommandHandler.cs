using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderHistoryService.Command;
using Shared.DTOs;

namespace OrderHistoryService.Handler
{
    public class OrderUserHistoryCommandHandler : IRequestHandler<GetOrderHistoryQuery, List<OrderUserHistoryDTO>>, IDisposable
    {
        private readonly IOrderRepository _OrderRepository;

        public OrderUserHistoryCommandHandler(IOrderRepository OrderRepository)
        {
            _OrderRepository = OrderRepository;
        }

       

        public void Dispose()
        {
            _OrderRepository.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task<List<OrderUserHistoryDTO>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

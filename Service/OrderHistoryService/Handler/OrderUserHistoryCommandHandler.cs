using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderHistoryService.Command;
using Shared.DTOs;

namespace OrderHistoryService.Handler
{
    public class OrderUserHistoryCommandHandler : IRequestHandler<GetOrderHistoryQuery, List<OrderUserHistoryDTO>>, IDisposable
    {
        private readonly IOrderHistoryRepository _orderHistoryRepository;

        public OrderUserHistoryCommandHandler(IOrderHistoryRepository OrderHistoryRepository)
        {
            _orderHistoryRepository = OrderHistoryRepository;
        }

   
        public void Dispose()
        {
            _orderHistoryRepository.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<List<OrderUserHistoryDTO>> Handle(GetOrderHistoryQuery request, CancellationToken cancellationToken)
{
            return await _orderHistoryRepository.GetOrderHistoryAsync(request.UserId);
        }
    }
}

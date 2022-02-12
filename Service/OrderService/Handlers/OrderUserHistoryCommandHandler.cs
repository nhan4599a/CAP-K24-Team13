using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderHistoryService.Commands;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderHistoryService.Handlers
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

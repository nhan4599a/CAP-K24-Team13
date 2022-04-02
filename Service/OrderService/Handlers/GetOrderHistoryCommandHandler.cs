using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class GetOrderHistoryCommandHandler : IRequestHandler<GetOrderHistoryByUserIdQuery, List<OrderItemDTO>>, IDisposable
    {
        private readonly IInvoiceRepository _orderRepository;

        public GetOrderHistoryCommandHandler(IInvoiceRepository OrderHistoryRepository)
        {
            _orderRepository = OrderHistoryRepository;
        }

        public async Task<List<OrderItemDTO>> Handle(GetOrderHistoryByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetOrderHistoryAsync(request.UserId);
        }

        public void Dispose()
        {
            _orderRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

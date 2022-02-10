using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared;
using Shared.DTOs;

namespace OrderService.Handlers
{
    public class GetOrderDetailHandler : IRequestHandler<GetOrderDetailQuery, CommandResponse<InvoiceDTO>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderDetailHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<CommandResponse<InvoiceDTO>> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetOrderDetailAsync(request.OrderId);
        }
    }
}

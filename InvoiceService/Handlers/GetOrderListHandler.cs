using DatabaseAccessor.Repositories.Abstraction;
using InvoiceService.Commands;
using MediatR;
using Shared;
using Shared.DTOs;

namespace InvoiceService.Handlers
{
    public class GetOrderListHandler : IRequestHandler<GetOrderListQuery, CommandResponse<List<InvoiceDTO>>>
    {

        private readonly IOrderRepository _orderRepository;
        public GetOrderListHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<CommandResponse<List<InvoiceDTO>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            return await _orderRepository.GetOrderListAsync(request.RequestModel);
        }
    }
}

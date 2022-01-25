using CheckoutService.Commands;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared;

namespace CheckoutService.Handlers
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IOrderRepository _orderRepository;

        public CheckOutCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CommandResponse<bool>> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            return await _orderRepository.AddOrderAsync(request.UserId, request.ProductIds, request.ShippingAddress);
        }

        public void Dispose()
        {
            _orderRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

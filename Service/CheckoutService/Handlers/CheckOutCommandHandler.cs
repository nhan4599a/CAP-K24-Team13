using CheckoutService.Commands;
using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CheckoutService.Handlers
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IInvoiceRepository _orderRepository;

        public CheckOutCommandHandler(IInvoiceRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CommandResponse<bool>> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            return await _orderRepository.AddOrderAsync(request.UserId, request.ProductIds, request.ShippingName,
                request.ShippingPhone, request.ShippingAddress, request.OrderNotes);
        }

        public void Dispose()
        {
            _orderRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

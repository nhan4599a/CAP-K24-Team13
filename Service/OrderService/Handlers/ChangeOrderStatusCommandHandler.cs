using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class ChangeOrderStatusCommandHandler : IRequestHandler<ChangeOrderStatusCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public ChangeOrderStatusCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<bool>> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
        {
            return await _invoiceRepository.ChangeOrderStatusAsync(request.InvoiceId, request.NewStatus);
        }

        public void Dispose()
        {
            _invoiceRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

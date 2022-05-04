using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using InvoiceService.Commands;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class ChangeInvoiceStatusCommandHandler : IRequestHandler<ChangeInvoiceStatusCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public ChangeInvoiceStatusCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CommandResponse<bool>> Handle(ChangeInvoiceStatusCommand request, CancellationToken cancellationToken)
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

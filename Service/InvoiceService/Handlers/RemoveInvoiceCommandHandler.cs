using DatabaseAccessor.Repositories.Abstraction;
using InvoiceService.Commands;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class RemoveInvoiceCommandHandler : IRequestHandler<RemoveInvoiceCommand>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public RemoveInvoiceCommandHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(RemoveInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _repository.RemoveInvoiceAsync(request.RefId);
            return Unit.Value;
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

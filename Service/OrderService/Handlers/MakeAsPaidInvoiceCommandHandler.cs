using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class MakeAsPaidInvoiceCommandHandler : IRequestHandler<MakeAsPaidInvoiceCommand>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public MakeAsPaidInvoiceCommandHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(MakeAsPaidInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _repository.MakeAsPaidAsync(request.RefId);
            return Unit.Value;
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

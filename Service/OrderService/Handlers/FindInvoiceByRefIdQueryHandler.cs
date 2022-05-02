using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class FindInvoiceByRefIdQueryHandler : IRequestHandler<FindInvoiceByRefIdQuery, InvoiceDetailDTO[]>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public FindInvoiceByRefIdQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<InvoiceDetailDTO[]> Handle(FindInvoiceByRefIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetInvoiceDetailByRefIdAsync(request.RefId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

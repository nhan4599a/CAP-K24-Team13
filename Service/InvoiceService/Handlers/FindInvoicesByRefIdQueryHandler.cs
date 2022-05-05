using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using InvoiceService.Commands;
using Shared.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class FindInvoicesByRefIdQueryHandler 
        : IRequestHandler<FindInvoicesByRefIdQuery, InvoiceWithItemDTO[]>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public FindInvoicesByRefIdQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<InvoiceWithItemDTO[]> Handle(FindInvoicesByRefIdQuery request,
            CancellationToken cancellationToken)
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

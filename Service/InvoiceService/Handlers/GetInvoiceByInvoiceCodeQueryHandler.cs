using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using InvoiceService.Commands;
using Shared.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class GetInvoiceByInvoiceCodeQueryHandler
        : IRequestHandler<GetInvoiceByInvoiceCodeQuery, FullInvoiceDTO>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public GetInvoiceByInvoiceCodeQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<FullInvoiceDTO> Handle(GetInvoiceByInvoiceCodeQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetInvoiceDetailAsync(request.InvoiceCode);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

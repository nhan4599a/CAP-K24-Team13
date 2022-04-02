using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared.DTOs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class GetInvoiceByInvoiceCodeQueryHandler
        : IRequestHandler<GetInvoiceByInvoiceCodeQuery, InvoiceDetailDTO>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public GetInvoiceByInvoiceCodeQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<InvoiceDetailDTO> Handle(GetInvoiceByInvoiceCodeQuery request, CancellationToken cancellationToken)
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

using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class FindInvoiceQueryHandler : IRequestHandler<FindInvoiceQuery, CommandResponse<PaginatedList<InvoiceDTO>>>,
        IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public FindInvoiceQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PaginatedList<InvoiceDTO>>> Handle(FindInvoiceQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindInvoicesAsync(request.Key, request.Value!.ToString(), new PaginationInfo
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

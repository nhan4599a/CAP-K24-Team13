using DatabaseAccessor.Repositories.Abstraction;
using InvoiceService.Commands;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class FindInvoicesQueryHandler : IRequestHandler<FindInvoiceQuery, CommandResponse<PaginatedList<InvoiceWithReportDTO>>>,
        IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public FindInvoicesQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<PaginatedList<InvoiceWithReportDTO>>> Handle(FindInvoiceQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.FindInvoicesAsync(request.ShopId, request.Key, request.Value!, new PaginationInfo
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

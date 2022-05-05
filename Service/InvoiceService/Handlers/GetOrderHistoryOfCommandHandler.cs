using DatabaseAccessor.Repositories.Abstraction;
using InvoiceService.Commands;
using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class GetOrderHistoryCommandHandler 
        : IRequestHandler<GetOrderHistoryQuery, Dictionary<string, InvoiceWithItemDTO[]>>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public GetOrderHistoryCommandHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<Dictionary<string, InvoiceWithItemDTO[]>> Handle(GetOrderHistoryQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetOrderHistoryAsync(request.UserId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

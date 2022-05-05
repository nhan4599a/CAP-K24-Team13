using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using InvoiceService.Commands;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceService.Handlers
{
    public class GetNearByInvoicesOfShopQueryHandler : IRequestHandler<GetNearByInvoicesOfShopQuery, List<InvoiceDTO>>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public GetNearByInvoicesOfShopQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<InvoiceDTO>> Handle(GetNearByInvoicesOfShopQuery request, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _repository.GetOrdersOfShopWithInTimeAsync(request.ShopId, today.AddDays(-30), today);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

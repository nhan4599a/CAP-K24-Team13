using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using OrderService.Commands;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Handlers
{
    public class GetNearByOrdersOfShopQueryHandler : IRequestHandler<GetNearByOrdersOfShopQuery, List<OrderDTO>>, IDisposable
    {
        private readonly IInvoiceRepository _repository;

        public GetNearByOrdersOfShopQueryHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDTO>> Handle(GetNearByOrdersOfShopQuery request, CancellationToken cancellationToken)
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

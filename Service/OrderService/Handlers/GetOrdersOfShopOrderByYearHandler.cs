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
    public class GetOrdersOfShopOrderByYearHandler : IRequestHandler<GetOrdersOfShopOrderByYearQuery, List<OrderDTO>>
    {
        private readonly IInvoiceRepository _repository;

        public GetOrdersOfShopOrderByYearHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDTO>> Handle(GetOrdersOfShopOrderByYearQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetOrdersOfShopByYearAsync(request.ShopId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

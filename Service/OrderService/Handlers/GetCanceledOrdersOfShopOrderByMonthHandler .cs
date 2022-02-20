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
    public class GetCanceledOrdersOfShopOrderByMonthHandler : IRequestHandler<GetCanceledOrdersOfShopOrderByMonthQuery, List<OrderDTO>>
    {
        private readonly IInvoiceRepository _repository;

        public GetCanceledOrdersOfShopOrderByMonthHandler(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<OrderDTO>> Handle(GetCanceledOrdersOfShopOrderByMonthQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCanceledOrdersOfShopByMonthAsync(request.ShopId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

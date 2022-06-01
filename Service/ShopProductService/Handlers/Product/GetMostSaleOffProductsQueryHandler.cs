using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class GetMostSaleOffProductsQueryHandler 
        : IRequestHandler<GetMostSaleOffProductsQuery, List<MinimalProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public GetMostSaleOffProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<List<MinimalProductDTO>> Handle(GetMostSaleOffProductsQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetTopMostSaleOffProductsAsync();
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

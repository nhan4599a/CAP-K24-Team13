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
    public class FindBestSellerProductsQueryHandler
        : IRequestHandler<FindBestSellerProductsQuery, List<MinimalProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindBestSellerProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MinimalProductDTO>> Handle(FindBestSellerProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetBestSellerProductsAsync(request.ShopId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

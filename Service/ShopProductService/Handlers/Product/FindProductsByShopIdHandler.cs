using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Product;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Product
{
    public class FindProductsByShopIdHandler
        : IRequestHandler<FindProductsByShopIdQuery, PaginatedList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _productRepository;

        public FindProductsByShopIdHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(FindProductsByShopIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllProductsOfShopAsync(request.ShopId, request.PaginationInfo);
        }

        public void Dispose()
        {
            _productRepository.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}

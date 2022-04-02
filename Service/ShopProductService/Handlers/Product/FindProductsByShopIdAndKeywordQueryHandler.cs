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
    public class FindProductsByShopIdAndKeywordQueryHandler :
        IRequestHandler<FindProductsByShopIdAndKeywordQuery, PaginatedList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindProductsByShopIdAndKeywordQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(FindProductsByShopIdAndKeywordQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.FindProductsOfShopAsync(request.ShopId, request.Keyword, request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

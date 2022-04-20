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
    public class FindProductsByCategoryIdQueryHandler
        : IRequestHandler<FindProductsByCategoryIdQuery, PaginatedList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public FindProductsByCategoryIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(FindProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Keyword != null)
                request.Keyword = request.Keyword.Trim();
            return await _repository.GetProductsOfCategoryAsync(null, request.CategoryIds, request.Keyword, request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

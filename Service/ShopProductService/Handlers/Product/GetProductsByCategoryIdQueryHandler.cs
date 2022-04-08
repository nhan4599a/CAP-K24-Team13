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
    public class GetProductsByCategoryIdQueryHandler
        : IRequestHandler<GetProductsByCategoryIdQuery, PaginatedList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public GetProductsByCategoryIdQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<ProductDTO>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetProductsOfCategoryAsync(request.CategoryId, request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

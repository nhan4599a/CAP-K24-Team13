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
    public class GetProductsOfShopInCategoryQueryHandler
        : IRequestHandler<GetProductsOfShopInCategoryQuery, PaginatedList<ProductDTO>>, IDisposable
    {
        private readonly IProductRepository _repository;

        public GetProductsOfShopInCategoryQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<PaginatedList<ProductDTO>> Handle(GetProductsOfShopInCategoryQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetProductsOfCategoryAsync(request.ShopId, request.CategoryIds, request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

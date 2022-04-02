using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Category;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class FindCategoriesByShopIdQueryHandler : IRequestHandler<FindCategoriesByShopIdQuery,
        PaginatedList<CategoryDTO>>, IDisposable
    {
        private readonly ICategoryRepository _repository;

        public FindCategoriesByShopIdQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<CategoryDTO>> Handle(FindCategoriesByShopIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetCategoriesOfShopAsync(request.ShopId, request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

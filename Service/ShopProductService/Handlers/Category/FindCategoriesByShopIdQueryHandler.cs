using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Category;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class FindCategoriesByShopIdQueryHandler : IRequestHandler<FindCategoriesByShopIdQuery, PaginatedList<CategoryDTO>>, IDisposable
    {
        private readonly ICategoryRepository _categoryRepository;

        public FindCategoriesByShopIdQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<PaginatedList<CategoryDTO>> Handle(FindCategoriesByShopIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            _categoryRepository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

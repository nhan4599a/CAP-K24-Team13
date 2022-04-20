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
    public class FindAllCategoriesQueryHandler
        : IRequestHandler<FindAllCategoriesQuery, PaginatedList<CategoryDTO>>, IDisposable
    {
        private readonly ICategoryRepository _repository;

        public FindAllCategoriesQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<CategoryDTO>> Handle(FindAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCategoriesAsync(request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

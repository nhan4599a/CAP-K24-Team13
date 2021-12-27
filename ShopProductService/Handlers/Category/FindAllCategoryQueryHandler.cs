using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using Shared.DTOs;
using ShopProductService.Commands.Category;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class FindAllCategoryQueryHandler : 
        IRequestHandler<FindAllCategoryQuery, PaginatedDataList<CategoryDTO>>, IDisposable
    {
        private readonly ICategoryRepository _repository;

        public FindAllCategoryQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedDataList<CategoryDTO>> Handle(FindAllCategoryQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetAllCategoryAsync(request.PaginationInfo);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

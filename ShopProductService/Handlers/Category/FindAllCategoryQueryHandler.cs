using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Category;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class FindAllCategoryQueryHandler : IRequestHandler<FindAllCategoryQuery, List<CategoryDTO>>, IDisposable
    {
        private readonly ICategoryRepository _repository;

        public FindAllCategoryQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CategoryDTO>> Handle(FindAllCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllCategoryAsync();
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

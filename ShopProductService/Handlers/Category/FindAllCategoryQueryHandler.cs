using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Category;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class FindAllCategoryQueryHandler : IRequestHandler<FindAllCategoryQuery, List<CategoryDTO>>
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
    }
}

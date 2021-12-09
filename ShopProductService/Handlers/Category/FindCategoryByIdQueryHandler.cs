using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared.DTOs;
using ShopProductService.Commands.Category;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class FindCategoryByIdQueryHandler : IRequestHandler<FindCategoryByIdQuery, CategoryDTO>
    {
        private readonly ICategoryRepository _repository;
        public FindCategoryByIdQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoryDTO> Handle(FindCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetCategoryAsync(request.Id);
        }
    }
}

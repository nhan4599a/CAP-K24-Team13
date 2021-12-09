using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Category;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CommandResponse<bool>>
    {
        private readonly ICategoryRepository _repository;

        public AddCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AddCategoryAsync(request.RequestModel);
        }
    }
}

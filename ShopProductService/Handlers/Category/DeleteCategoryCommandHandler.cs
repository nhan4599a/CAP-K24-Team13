using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Category;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CommandResponse<bool>>
    {
        private readonly ICategoryRepository _repository;

        public DeleteCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ActiveCategoryAsync(request.Id, true);
        }
    }
}

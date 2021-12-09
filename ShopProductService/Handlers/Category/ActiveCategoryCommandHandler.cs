using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Category;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class ActiveCategoryCommandHandler : IRequestHandler<ActiveCategoryCommand, CommandResponse<bool>>
    {
        private readonly ICategoryRepository _repository;

        public ActiveCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(ActiveCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ActiveCategoryAsync(request.Id, false);
        }
    }
}

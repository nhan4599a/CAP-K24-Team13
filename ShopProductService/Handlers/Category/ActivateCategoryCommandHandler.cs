using DatabaseAccessor.Repositories.Interfaces;
using MediatR;
using Shared;
using ShopProductService.Commands.Category;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class ActivateCategoryCommandHandler : IRequestHandler<ActivateCategoryCommand, CommandResponse<bool>>
    {
        private readonly ICategoryRepository _repository;

        public ActivateCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(ActivateCategoryCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ActivateCategoryAsync(request.Id, request.IsActivateCommand, request.ShouldBeCascade);
        }
    }
}

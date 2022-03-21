using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using ShopProductService.Commands.Category;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShopProductService.Handlers.Category
{
    public class ActivateCategoryCommandHandler : 
        IRequestHandler<ActivateCategoryCommand, CommandResponse<bool>>, IDisposable
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

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

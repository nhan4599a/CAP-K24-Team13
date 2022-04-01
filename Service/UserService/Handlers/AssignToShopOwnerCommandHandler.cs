using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserService.Commands;

namespace UserService.Handlers
{
    public class AssignToShopOwnerCommandHandler : IRequestHandler<AssignToShopOwnerCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IUserRepository _repository;

        public AssignToShopOwnerCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(AssignToShopOwnerCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AssignShopOwnerAsync(request.UserId, request.ShopId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserService.Commands;

namespace UserService.Handlers
{
    public class BanUserCommandHandler : IRequestHandler<BanUserCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IUserRepository _repository;

        public BanUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            return await _repository.ApplyBanAsync(request.UserId, request.Behavior);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

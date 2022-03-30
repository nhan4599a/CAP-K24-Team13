using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserService.Commands;

namespace UserService.Handlers
{
    public class UnbanUserCommandHandler : IRequestHandler<UnbanUserCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IUserRepository _repository;

        public UnbanUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(UnbanUserCommand request, CancellationToken cancellationToken)
        {
            return await _repository.UnbanAsync(request.UserId);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

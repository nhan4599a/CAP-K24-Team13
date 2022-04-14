using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserService.Commands;

namespace UserService.Handlers
{
    public class AuthorizeUserCommandHandler : IRequestHandler<AuthorizeUserCommand, CommandResponse<bool>>, IDisposable
    {
        private readonly IUserRepository _repository;

        public AuthorizeUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<CommandResponse<bool>> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
        {
            return await _repository.AuthorizeUserAsync(request.UserId, request.AuthorizeToAdmin, request.ToTeam5Admin);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

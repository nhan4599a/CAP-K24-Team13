using DatabaseAccessor.Repositories.Abstraction;
using MediatR;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UserService.Commands;

namespace UserService.Handlers
{
    public class FindUsersQueryHandler : IRequestHandler<FindUsersQuery, PaginatedList<UserDTO>>, IDisposable
    {
        private readonly IUserRepository _repository;

        public FindUsersQueryHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<UserDTO>> Handle(FindUsersQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindUsersAsync(request.Keyword, new PaginationInfo
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            }, request.RoleName);
        }

        public void Dispose()
        {
            _repository.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

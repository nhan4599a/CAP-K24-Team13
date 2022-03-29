using DatabaseAccessor.Contexts;
using DatabaseAccessor.Mapping;
using DatabaseAccessor.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseAccessor.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly Mapper _mapper;

        public UserRepository(ApplicationDbContext dbContext, Mapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedList<UserDTO>> GetAllUsersAsync(PaginationInfo paginationInfo, bool customer)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Include(e => e.UserRoles)
                .ThenInclude(e => e.Role)
                .Where(e => !customer || e.UserRoles.Any(userRole => userRole.Role.Name == Roles.CUSTOMER))
                .Select(e => _mapper.MapToUserDTO(e))
                .PaginateAsync(paginationInfo.PageNumber, paginationInfo.PageSize);
        }

        public async Task<CommandResponse<bool>> ApplyBanAsync(Guid userId, AccountPunishmentBehavior behavior)
        {
            var user = await _dbContext.Users.FindAsync(userId);

            if (user == null)
                return CommandResponse<bool>.Error("User not found", null);

            if (user.LockoutEnd >= DateTimeOffset.Now)
                return CommandResponse<bool>.Error("User is already ban", null);

            if (behavior == AccountPunishmentBehavior.SendAlertEmail)
                return CommandResponse<bool>.Error("Behavior is not supported", new NotSupportedException());

            user.LockoutEnd = behavior switch
            {
                AccountPunishmentBehavior.LockedOut => DateTimeOffset.Now.AddDays(14),
                AccountPunishmentBehavior.LockedOutPermanently => null,
                _ => throw new NotSupportedException()
            };

            user.Status = AccountStatus.Banned;

            await _dbContext.SaveChangesAsync();

            return CommandResponse<bool>.Success(true);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using EFCore.BulkExtensions;
using Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Background
{
    public class AccountStatusUpdateBackgroundJob
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountStatusUpdateBackgroundJob(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DoJob()
        {
            await _dbContext.Users
                .Where(user => user.LockoutEnd != null && user.LockoutEnd < DateTimeOffset.Now)
                .BatchUpdateAsync(new User { Status = AccountStatus.Available });
        }
    }
}

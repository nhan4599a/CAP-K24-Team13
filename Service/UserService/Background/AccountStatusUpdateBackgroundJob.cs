using DatabaseAccessor.Contexts;
using Shared.Models;
using System;
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

        public async Task DoJob(string userId)
        {
            var parseResult = Guid.TryParse(userId, out Guid guid);
            if (parseResult)
            {
                var user = await _dbContext.Users.FindAsync(guid);
                if (user != null)
                {
                    user.Status = AccountStatus.Available;
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}

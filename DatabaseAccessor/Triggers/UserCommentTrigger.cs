using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using EntityFrameworkCore.Triggered;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    internal class UserCommentTrigger : IBeforeSaveTrigger<ProductComment>, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public UserCommentTrigger(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task BeforeSave(ITriggerContext<ProductComment> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added)
            {

            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

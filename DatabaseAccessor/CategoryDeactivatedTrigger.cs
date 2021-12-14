using DatabaseAccessor.Models;
using EFCore.BulkExtensions;
using EntityFrameworkCore.Triggered;
using EntityFrameworkCore.Triggered.Transactions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor
{
    internal class CategoryDeactivatedTrigger : IAfterCommitTrigger<ShopCategory>
    {
        public readonly ApplicationDbContext _dbContext;

        public CategoryDeactivatedTrigger(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AfterCommit(ITriggerContext<ShopCategory> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Modified && context.Entity.IsDisabled)
            {
                await _dbContext.ShopProducts
                    .Where(product => product.CategoryId == context.Entity.Id)
                    .BatchUpdateAsync(new ShopProduct { IsDisabled = true }, cancellationToken: CancellationToken.None);
            }
        }
    }
}

using DatabaseAccessor.Models;
using EFCore.BulkExtensions;
using EntityFrameworkCore.Triggered;
using EntityFrameworkCore.Triggered.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    internal class CategoryActivatedTrigger : IAfterCommitTrigger<ShopCategory>, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryActivatedTrigger(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AfterCommit(ITriggerContext<ShopCategory> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Modified && !context.Entity.IsDisabled)
            {
                await _dbContext.ShopProducts
                    .Where(product => product.CategoryId == context.Entity.Id)
                    .BatchUpdateAsync(
                        new ShopProduct { IsDisabled = false },
                        new List<string> { "IsDisabled" },
                        cancellationToken
                    );
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

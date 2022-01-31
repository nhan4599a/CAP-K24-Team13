using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using EntityFrameworkCore.Triggered;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    public class InvoiceAddedTrigger : IBeforeSaveTrigger<Invoice>, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceAddedTrigger(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task BeforeSave(ITriggerContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added)
            {
                context.Entity.InvoiceCode = context.Entity.ShopId.ToString();
                context.Entity.InvoiceCode = context.Entity.Created.Date.ToString();
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

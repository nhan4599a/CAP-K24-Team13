using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using EntityFrameworkCore.Triggered;
using Shared.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    public class InvoiceStatusChangedTrigger : IBeforeSaveTrigger<Invoice>, IAfterSaveTrigger<Invoice>, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceStatusChangedTrigger(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AfterSave(ITriggerContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added)
            {
                var invoice = await _dbContext.Invoices.FindAsync(new object[] { context.Entity.Id }, cancellationToken: cancellationToken);
                invoice.StatusChangedHistories.Add(new InvoiceStatusChangedHistory
                {
                    OldStatus = null,
                    NewStatus = InvoiceStatus.New
                });
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task BeforeSave(ITriggerContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Modified)
            {
                context.Entity.StatusChangedHistories.Add(new InvoiceStatusChangedHistory
                {
                    OldStatus = context.UnmodifiedEntity.Status,
                    NewStatus = context.Entity.Status
                });
                if (context.Entity.Status == InvoiceStatus.Confirmed && context.UnmodifiedEntity.Status == InvoiceStatus.New)
                {
                    foreach (var detail in context.Entity.Details)
                    {
                        detail.Product.Quantity -= detail.Quantity;
                    }
                }

                if (context.Entity.Status == InvoiceStatus.Canceled)
                {
                    foreach (var detail in context.Entity.Details)
                    {
                        detail.Product.Quantity += detail.Quantity;
                    }
                }
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

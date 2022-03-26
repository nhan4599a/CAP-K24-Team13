using DatabaseAccessor.Models;
using EntityFrameworkCore.Triggered;
using Shared.Models;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    internal class InvoiceStatusChangedTrigger : IBeforeSaveTrigger<Invoice>
    {
        public Task BeforeSave(ITriggerContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Modified || context.ChangeType == ChangeType.Added)
            {
                if (context.ChangeType == ChangeType.Added)
                {
                    context.Entity.StatusChangedHistories.Add(new InvoiceStatusChangedHistory
                    {
                        OldStatus = null,
                        NewStatus = InvoiceStatus.New
                    });
                }
                else
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
            }
            return Task.CompletedTask;
        }
    }
}

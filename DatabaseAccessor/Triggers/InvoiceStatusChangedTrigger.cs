using DatabaseAccessor.Models;
using EntityFrameworkCore.Triggered;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    internal class InvoiceStatusChangedTrigger : IBeforeSaveTrigger<Invoice>
    {
        public Task BeforeSave(ITriggerContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Modified && context.Entity.Status != context.UnmodifiedEntity.Status)
            {
                context.Entity.StatusChangedHistory.Add(new InvoiceStatusChangedHistory
                {
                    OldStatus = context.UnmodifiedEntity.Status,
                    NewStatus = context.Entity.Status
                });
            }
            return Task.CompletedTask;
        }
    }
}

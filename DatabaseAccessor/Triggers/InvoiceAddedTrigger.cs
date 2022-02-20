using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using EntityFrameworkCore.Triggered;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseAccessor.Triggers
{
    public class InvoiceAddedTrigger : IAfterSaveTrigger<Invoice>, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public InvoiceAddedTrigger(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AfterSave(ITriggerContext<Invoice> context, CancellationToken cancellationToken)
        {
            if (context.ChangeType == ChangeType.Added)
            {
                var count = _dbContext.Invoices
                    .Count(invoice => invoice.CreatedAt.Date == context.Entity.CreatedAt.Date && invoice.ShopId == context.Entity.ShopId);
                var invoice = await _dbContext.Invoices.FindAsync(new object[] { context.Entity.Id }, cancellationToken: cancellationToken);
                invoice.InvoiceCode = invoice.ShopId.ToString();
                invoice.InvoiceCode += invoice.CreatedAt.Date.ToString("ddMMyyyy");
                invoice.InvoiceCode += count.ToString("00000");
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

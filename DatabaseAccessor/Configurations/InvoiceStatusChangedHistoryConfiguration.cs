using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceStatusChangedHistoryConfiguration : IEntityTypeConfiguration<InvoiceStatusChangedHistory>
    {
        public void Configure(EntityTypeBuilder<InvoiceStatusChangedHistory> builder)
        {
            builder.Property(e => e.ChangedDate)
                .HasDefaultValueSql("getdate()");

            builder.HasOne(e => e.Invoice)
                .WithMany(e => e.StatusChangedHistory)
                .HasForeignKey(e => e.InvoiceId);
        }
    }
}

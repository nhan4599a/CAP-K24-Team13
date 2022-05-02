using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceStatusChangedHistoryConfiguration : IEntityTypeConfiguration<InvoiceStatusChangedHistory>
    {
        public void Configure(EntityTypeBuilder<InvoiceStatusChangedHistory> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.Property(e => e.ChangedDate)
                .HasDefaultValueSql("getdate()");

            builder.HasOne(e => e.Invoice)
                .WithMany(e => e.StatusChangedHistories)
                .HasForeignKey(e => e.InvoiceId);
        }
    }
}

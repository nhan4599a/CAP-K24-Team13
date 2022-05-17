using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceStatusChangedHistoryConfiguration : BaseEntityConfiguration<InvoiceStatusChangedHistory>
    {
        private readonly bool _isForTestingPurpose;

        public InvoiceStatusChangedHistoryConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<InvoiceStatusChangedHistory> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            if (!IsForTestingPurpose)
            {
                builder.Property(e => e.ChangedDate)
                    .HasDefaultValueSql("getdate()");
            }

            builder.HasOne(e => e.Invoice)
                .WithMany(e => e.StatusChangedHistories)
                .HasForeignKey(e => e.InvoiceId);
        }
    }
}
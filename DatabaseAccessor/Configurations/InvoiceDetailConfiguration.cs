using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceDetailConfiguration : BaseEntityConfiguration<InvoiceDetail>
    {
        private readonly bool _isForTestingPurpose;

        public InvoiceDetailConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.Property(e => e.Quantity)
                .HasDefaultValue(1);

            builder.Property(e => e.IsRated)
                .HasDefaultValue(false);

            builder.HasCheckConstraint("CK_InvoiceDetail_Quantity", "[Quantity] >= 1")
                .HasCheckConstraint("CK_InvoiceDetail_Price", "[Price] > 0");
        }
    }
}

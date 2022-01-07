using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.Property(e => e.Quantity)
                .HasDefaultValue(1);

            builder.HasCheckConstraint("CK_InvoiceDetail_Quantity", "[Quantity] between 1 and 10")
                .HasCheckConstraint("CK_InvoiceDetail_Price", "[Price] > 0");
        }
    }
}

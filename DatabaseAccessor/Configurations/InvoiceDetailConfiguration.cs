using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.Property(e => e.Quantity)
                .HasDefaultValue(1);

            builder.HasCheckConstraint("CK_InvoiceDetail_Quantity", "[Quantity] >= 1")
                .HasCheckConstraint("CK_InvoiceDetail_Price", "[Price] > 0");
        }
    }
}

using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ShopProduct>
    {
        public void Configure(EntityTypeBuilder<ShopProduct> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(e => e.Category)
                .WithMany(e => e.ShopProducts)
                .IsRequired();

            builder.HasMany(e => e.Invoices)
                .WithOne(e => e.Product)
                .IsRequired();

            builder.Property(e => e.ProductName)
                .IsRequired();

            builder.Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            builder.Property(e => e.CreatedDate)
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.Discount)
                .HasDefaultValue(0);

            builder.Property(e => e.Quantity)
                .HasDefaultValue(1);

            builder.HasIndex(e => e.ProductName);

            builder.HasCheckConstraint("CK_ShopProducts_Price", "[Price] >= 0")
                .HasCheckConstraint("CK_ShopProducts_Quantity", "[Quantity] >= 1")
                .HasCheckConstraint("CK_ShopProducts_Discount", "[Discount] between 0 and 100");
        }
    }
}

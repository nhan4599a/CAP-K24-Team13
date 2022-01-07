using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<ShopCategory>
    {
        public void Configure(EntityTypeBuilder<ShopCategory> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            builder.HasIndex(e => e.CategoryName);
        }
    }
}

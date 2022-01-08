using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class ShopInterfaceConfiguration : IEntityTypeConfiguration<ShopInterface>
    {
        public void Configure(EntityTypeBuilder<ShopInterface> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.HasIndex(e => e.ShopId)
                .IsUnique();
        }
    }
}

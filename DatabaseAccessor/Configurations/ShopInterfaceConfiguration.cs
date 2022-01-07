using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class ShopInterfaceConfiguration : IEntityTypeConfiguration<ShopInterface>
    {
        public void Configure(EntityTypeBuilder<ShopInterface> builder)
        {
            builder.HasIndex(e => e.ShopId)
                .IsUnique();
        }
    }
}

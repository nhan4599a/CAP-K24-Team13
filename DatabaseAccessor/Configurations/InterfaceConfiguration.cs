using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InterfaceConfiguration : IEntityTypeConfiguration<ShopInterface>
    {
        public void Configure(EntityTypeBuilder<ShopInterface> builder)
        {
            builder.Property(e => e.IsVisible)
                .HasDefaultValue(true);
        }
    }
}

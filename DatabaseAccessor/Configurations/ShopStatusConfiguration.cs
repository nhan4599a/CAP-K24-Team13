using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class ShopStatusConfiguration : IEntityTypeConfiguration<ShopStatus>
    {
        public void Configure(EntityTypeBuilder<ShopStatus> builder)
        {
            builder.Property(e => e.IsDisabled)
                .HasDefaultValue(false);
        }
    }
}

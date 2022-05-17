using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class ShopStatusConfiguration : BaseEntityConfiguration<ShopStatus>
    {
        private readonly bool _isForTestingPurpose;

        public ShopStatusConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<ShopStatus> builder)
        {
            builder.Property(e => e.IsDisabled)
                .HasDefaultValue(false);
        }
    }
}

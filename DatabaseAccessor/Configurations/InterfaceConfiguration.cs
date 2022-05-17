using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class InterfaceConfiguration : BaseEntityConfiguration<ShopInterface>
    {
        private readonly bool _isForTestingPurpose;

        public InterfaceConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<ShopInterface> builder)
        {
            builder.Property(e => e.IsVisible)
                .HasDefaultValue(true);
        }
    }
}

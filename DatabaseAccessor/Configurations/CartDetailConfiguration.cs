using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    internal class CartDetailConfiguration : BaseEntityConfiguration<CartDetail>
    {
        private readonly bool _isForTestingPurpose;

        public CartDetailConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.Property(e => e.Quantity)
                .HasDefaultValue(1);

            builder.HasOne(e => e.Cart)
                .WithMany(e => e.Details)
                .IsRequired();

            builder.HasOne(e => e.Product)
                .WithMany(e => e.CartDetails);

            builder.HasCheckConstraint("CK_CartDetail_Quantity", "[Quantity] >= 1");
        }
    }
}

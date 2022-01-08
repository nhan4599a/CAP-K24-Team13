using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.HasOne(e => e.User)
                .WithOne(e => e.Cart);
        }
    }
}

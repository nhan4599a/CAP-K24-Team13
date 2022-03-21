using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(200);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Status).HasDefaultValue(AccountStatus.Available);
        }
    }
}

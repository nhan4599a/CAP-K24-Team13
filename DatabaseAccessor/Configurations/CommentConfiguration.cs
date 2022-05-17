using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class CommentConfiguration : BaseEntityConfiguration<ProductComment>
    {
        private readonly bool _isForTestingPurpose;

        public CommentConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            if (!IsForTestingPurpose)
            {
                builder.Property(e => e.CreatedDate)
                    .HasDefaultValueSql("getdate()");
            }

            builder.HasOne(e => e.User)
                .WithMany()
                .IsRequired();

            builder.HasOne(e => e.Product)
                .WithMany(e => e.Comments)
                .IsRequired();

            builder.HasCheckConstraint("CK_ProductComments_Star", "[Star] between 1 and 5");
        }
    }
}

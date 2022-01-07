using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<ProductComment>
    {
        public void Configure(EntityTypeBuilder<ProductComment> builder)
        {
            builder.Property(e => e.CreatedDate)
                .HasDefaultValueSql("getdate()");

            builder.HasOne(e => e.User)
                .WithMany()
                .IsRequired();

            builder.HasOne(e => e.Product)
                .WithMany(e => e.Comments)
                .IsRequired();

            builder.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ReferenceId);

            builder.HasCheckConstraint("CK_ProductComments_Star", "[Star] between 1 and 5");
        }
    }
}

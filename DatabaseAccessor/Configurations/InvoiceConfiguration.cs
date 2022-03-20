using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace DatabaseAccessor.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.HasOne(e => e.User)
                .WithMany(e => e.Invoices)
                .IsRequired();

            builder.HasIndex(e => e.UserId);
            builder.HasIndex(e => e.CreatedAt);

            builder.HasMany(e => e.Details)
                .WithOne(e => e.Invoice)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.Status)
                .HasDefaultValue(InvoiceStatus.New);
        }
    }
}

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
            builder.HasOne(e => e.User)
                .WithMany(e => e.Invoices)
                .IsRequired();

            builder.HasIndex(e => new { e.UserId, e.Created });

            builder.HasMany(e => e.Details)
                .WithOne(e => e.Invoice)
                .IsRequired();

            builder.Property(e => e.Created)
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.Status)
                .HasDefaultValue(InvoiceStatus.New);
        }
    }
}

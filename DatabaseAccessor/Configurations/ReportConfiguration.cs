using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessor.Configurations
{
    public class ReportConfiguration : BaseEntityConfiguration<Report>
    {
        private readonly bool _isForTestingPurpose;

        public ReportConfiguration(bool isForTestingPurpose = false)
        {
            _isForTestingPurpose = isForTestingPurpose;
        }

        public override bool IsForTestingPurpose => _isForTestingPurpose;

        public override void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasIdentityOptions(0, 1);

            builder.HasOne(e => e.Reporter)
                .WithMany(e => e.Reports)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(e => e.AffectedUser)
                .WithMany(e => e.AffectedReports)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(e => e.AffectedInvoice)
                .WithOne(e => e.Report)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            if (!IsForTestingPurpose)
            {
                builder.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("getdate()");
            }

            builder.HasIndex(e => e.ReporterId);
            builder.HasIndex(e => new { e.AffectedUserId, e.CreatedAt });
        }
    }
}

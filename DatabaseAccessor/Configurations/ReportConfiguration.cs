﻿using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Models;

namespace DatabaseAccessor.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
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

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("getdate()");

            builder.Property(e => e.Status)
                .HasDefaultValue(ReportStatus.New);

            builder.HasIndex(e => e.ReporterId);
            builder.HasIndex(e => new { e.AffectedUserId, e.CreatedAt });
        }
    }
}
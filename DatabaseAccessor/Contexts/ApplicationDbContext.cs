using DatabaseAccessor.Configurations;
using DatabaseAccessor.Converters;
using DatabaseAccessor.Models;
using DatabaseAccessor.Triggers;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using System;

namespace DatabaseAccessor.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>, IDataProtectionKeyContext
    {
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("TEAM13_CONNECTION_STRING");

        public DbSet<ShopInterface> ShopInterfaces { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public DbSet<InvoiceStatusChangedHistory> InvoiceStatusChangedHistories { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartDetail> CartDetails { get; set; }

        public DbSet<Report> Reports { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<CategoryDTO> Categories { get; set; }

        public DbSet<ShopStatus> ShopStatus { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(string connectionString) : base(
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options)
        {
        }

        public ApplicationDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                        .UseLazyLoadingProxies()
                        .UseSqlServer(_connectionString)
                        .UseTriggers(options =>
                        {
                            options.UseTransactionTriggers();
                            options.AddTrigger<InvoiceAddedTrigger>();
                            options.AddTrigger<InvoiceStatusChangedTrigger>();
                            options.AddTrigger<BanOrUnbanShopOwnerTrigger>();
                        });
            }
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            configurationBuilder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var isForTestingPurpose = Database.IsSqlite();

            modelBuilder.ApplyConfiguration(new UserConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new RoleConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new ProductConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new InterfaceConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new InvoiceDetailConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new CommentConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new InvoiceStatusChangedHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new CartDetailConfiguration(isForTestingPurpose));
            modelBuilder.ApplyConfiguration(new ReportConfiguration(isForTestingPurpose));

            modelBuilder.Entity<CategoryDTO>()
                .HasNoKey();

            modelBuilder.Entity<ShopProduct>()
                .HasQueryFilter(e => e.IsVisible && !e.IsDisabled);
        }
    }
}

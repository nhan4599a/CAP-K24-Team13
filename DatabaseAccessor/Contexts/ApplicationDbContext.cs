using DatabaseAccessor.Configurations;
using DatabaseAccessor.Converters;
using DatabaseAccessor.Models;
using DatabaseAccessor.Triggers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseAccessor.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        //private static readonly string _connectionString = Environment.GetEnvironmentVariable("TEAM13_CONNECTION_STRING");

        private static readonly string _connectionString = "Server=.,4599;Database=DemoCapTeam13;User ID=sa;Password=nhan4599@Nhan;TrustServerCertificate=true";

        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<ShopInterface> ShopInterfaces { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public DbSet<ProductComment> ProductComments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext(string connectionString) : base(
            new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString).Options)
        { }

        public ApplicationDbContext() : base() { }

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
                            options.AddTrigger<CategoryActivatedTrigger>();
                            options.AddTrigger<CategoryDeactivatedTrigger>();
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

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ShopInterfaceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
        }
    }
}

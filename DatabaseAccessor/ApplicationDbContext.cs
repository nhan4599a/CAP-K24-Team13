using DatabaseAccessor.Configurations;
using DatabaseAccessor.Converters;
using DatabaseAccessor.Models;
using DatabaseAccessor.Triggers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseAccessor
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        //private static readonly string _connectionString = Environment.GetEnvironmentVariable("TEAM13_CONNECTION_STRING");

        private static readonly string _connectionString = "Server=.,4599;Database=Temp;User ID=sa;Password=nhan4599@Nhan;TrustServerCertificate=true";

        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<ShopInterface> ShopInterfaces { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public ApplicationDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                        .UseLazyLoadingProxies()
                        .UseOpenIddict()
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

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ShopProduct>()
                .HasOne(e => e.Category)
                .WithMany(e => e.ShopProducts)
                .IsRequired();

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.ProductName)
                .IsRequired();

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.Discount)
                .HasDefaultValue(0);

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.Quantity)
                .HasDefaultValue(1);

            modelBuilder.Entity<ShopProduct>()
                .HasIndex(e => e.ProductName);

            modelBuilder.Entity<ShopProduct>()
                .HasCheckConstraint("CK_ShopProducts_Price", "[Price] >= 0")
                .HasCheckConstraint("CK_ShopProducts_Quantity", "[Quantity] >= 1")
                .HasCheckConstraint("CK_ShopProducts_Discount", "[Discount] between 0 and 100");

            modelBuilder.Entity<ShopCategory>()
                .Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ShopCategory>()
                .HasIndex(e => e.CategoryName);

            modelBuilder.Entity<ShopInterface>()
                .HasIndex(e => e.ShopId)
                .IsUnique();
        }
    }
}

using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseAccessor
{

    public class ApplicationDbContext : DbContext
    {
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("TEAM13_CONNECTION_STRING");

        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<ShopInterface> ShopInterfaces { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(_connectionString)
                .UseTriggers(options =>
                {
                    options.UseTransactionTriggers();
                    options.AddTrigger<CategoryDeactivatedTrigger>();
                });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                .HasCheckConstraint("CK_ShopProducts_Discount", "[Discount] between 0 and 100")
                .ToTable("ShopProducts");

            modelBuilder.Entity<ShopCategory>()
                .Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ShopCategory>()
                .HasIndex(e => e.CategoryName);

            modelBuilder.Entity<ShopCategory>()
                .ToTable("ShopCategories");

            modelBuilder.Entity<ShopInterface>()
                .HasIndex(e => e.ShopId)
                .IsUnique();

            modelBuilder.Entity<ShopInterface>()
                .ToTable("ShopInterfaces");
        }
    }
}

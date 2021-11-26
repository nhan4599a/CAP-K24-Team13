using DatabaseAccessor.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor
{

    public class ApplicationDbContext : DbContext
    {
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("TEAM13_CONNECTION_STRING");

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<ShopImage> ShopImages { get; set; }

        public DbSet<ShopInterface> ShopInterfaces { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

        public DbSet<ShopCategory> Category { get; set; }

        public ApplicationDbContext() : base(GetOptions(_connectionString))
        {

        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShopProduct>()
                    .HasOne(e => e.ImageSet)
                    .WithOne(e => e.Product)
                    .HasForeignKey<ProductImage>(e => e.ShopProductId);
            modelBuilder.Entity<ShopProduct>()
                    .HasOne(e => e.Category)
                    .WithMany(e => e.ShopProducts)
                    .IsRequired();
            modelBuilder.Entity<ShopInterface>()
                    .HasOne(e => e.ShopImage)
                    .WithOne(e => e.ShopInterface)
                    .HasForeignKey<ShopImage>(e => e.ShopInterfaceId);
            modelBuilder.Entity<ShopCategory>(entity =>
            {
                entity.ToTable("ShopCategories");
            });
            modelBuilder.Entity<ShopProduct>(entity =>
            {
                entity.ToTable("ShopProducts");
            });
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.ToTable("ProductImages");
            });
            modelBuilder.Entity<ShopImage>(entity =>
            {
                entity.ToTable("ShopImages");
            });
            modelBuilder.Entity<ShopInterface>(entity =>
            {
                entity.ToTable("ShopInterfaces");
            });
        }

    }
}

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

        public DbSet<ShopCategory> ShopCategories { get; set; }

        public DbSet<ShopInterface> ShopInterfaces { get; set; }

        public DbSet<ShopProduct> ShopProducts { get; set; }

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
                    .Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            modelBuilder.Entity<ShopProduct>()
                    .HasOne(e => e.Category)
                    .WithMany(e => e.ShopProducts)
                    .IsRequired();
                    
            modelBuilder.Entity<ShopProduct>()
                    .Property(e => e.IsDisabled)
                    .HasDefaultValue(false);

            modelBuilder.Entity<ShopProduct>()
                    .Property(e => e.CreatedDate)
                    .HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<ShopCategory>(entity =>
            {
                entity.ToTable("ShopCategories");
            });

            modelBuilder.Entity<ShopProduct>(entity =>
            {
                entity.ToTable("ShopProducts");
            });

            modelBuilder.Entity<ShopInterface>(entity =>
            {
                entity.ToTable("ShopInterfaces");
            });
        }
    }
}

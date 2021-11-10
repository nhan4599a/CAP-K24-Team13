using DatabaseAccessor.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseAccessor
{
   
    public class ApplicationDbContext : DbContext
    {
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("TEAM13_CONNECTION_STRING");

        public DbSet<ProductImage> ProductImages;

        public DbSet<ShopCategory> ShopCategories;

        public DbSet<ShopImage> ShopImages;

        public DbSet<ShopInterface> ShopInterfaces;

        public DbSet<ShopProduct> ShopProducts;

        public ApplicationDbContext() : base(GetOptions(_connectionString))
        {

        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}

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


    }
}

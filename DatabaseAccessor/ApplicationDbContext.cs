using DatabaseAccessor.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccessor
{

    public class ApplicationDbContext : DbContext
    {
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("Data Source=DESKTOP-LABGN86;Initial Catalog=Team13-DB;Integrated Security=True");

        public DbSet<ProductImage> ProductImages;

        public DbSet<ShopCategory> ShopCategories;

        public DbSet<ShopImage> ShopImages;

        public DbSet<ShopInterface> ShopInterfaces;

        public DbSet<ShopProduct> ShopProducts;

        public DbSet<ShopCategory> Category { get; set; }

        public ApplicationDbContext() : base(GetOptions(_connectionString))
        {

        }

        public Task<IEnumerable<ShopCategory>> ToListAsync()
        {
            throw new NotImplementedException();
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }


    }
}

using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseSharing
{
    public class FakeApplicationDbContext : ApplicationDbContext
    {
        public FakeApplicationDbContext() : base(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(@"Data Source=UnitTest.db").Options)
        { }

        public static readonly List<ShopProduct> listProducts = new()
        {
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "Women fashion",
                CategoryId = 3,
                ProductName = "Women fashion",
                Description = "Clothes of women",
                Quantity = 15,
                Price = 200000,
                Discount = 50,
                ShopId = 1
            },
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "Men fashion",
                CategoryId = 2,
                ProductName = "Men fashion",
                Description = "Clothes of men",
                Quantity = 20,
                Price = 300000,
                Discount = 30,
                ShopId = 1
            },
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "Women accesories",
                CategoryId = 12,
                ProductName = "Women accessories",
                Description = "accesories of women",
                Quantity = 50,
                Price = 450000,
                Discount = 10,
                ShopId = 1
            },
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "Men accesories",
                CategoryId = 13,
                ProductName = "Men accessories",
                Description = "accesories of men",
                Quantity = 90,
                Price = 700000,
                Discount = 50,
                ShopId = 1
            },
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "Gamming",
                CategoryId = 26,
                ProductName = "Gamming",
                Description = "Laptop gamming",
                Quantity = 100,
                Price = 3200000,
                Discount = 40,
                ShopId = 1
            },
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "electric devices",
                CategoryId = 19,
                ProductName = "gamming gears",
                Description = "gamming gears",
                Quantity = 150,
                Price = 5000000,
                Discount = 20,
                ShopId = 1
            },
            new ShopProduct
            {
                Id = Guid.NewGuid(),
                Category = "Camera&Flycam",
                CategoryId = 20,
                ProductName = "camera devices",
                Description = "camera devices",
                Quantity = 190,
                Price = 9200000,
                Discount = 80,
                IsDisabled = true,
                ShopId = 2
            }
        };

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var shops = new List<ShopStatus>
            {
                new ShopStatus
                {
                    ShopId = 1,
                    IsDisabled = false
                },
                new ShopStatus
                {
                    ShopId = 2,
                    IsDisabled = true
                }
            };

            modelBuilder.Entity<ShopStatus>()
                .HasData(shops);

            modelBuilder.Entity<ShopProduct>()
                .HasData(listProducts);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public override int SaveChanges()
        {
            return 0;
        }
    }
}
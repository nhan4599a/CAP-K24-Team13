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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.ProductName)
                .IsRequired();

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.Discount)
                .HasDefaultValue(0);

            modelBuilder.Entity<ShopProduct>()
                .Property(e => e.Quantity)
                .HasDefaultValue(1);

            modelBuilder.Entity<ShopProduct>()
                .HasCheckConstraint("CK_ShopProducts_Price", "[Price] >= 0")
                .HasCheckConstraint("CK_ShopProducts_Quantity", "[Quantity] >= 1")
                .HasCheckConstraint("CK_ShopProducts_Discount", "[Discount] between 0 and 100")
                .ToTable("ShopProducts");

            modelBuilder.Entity<ShopCategory>()
                .Property(e => e.IsDisabled)
                .HasDefaultValue(false);

            modelBuilder.Entity<ShopCategory>()
                .ToTable("ShopCategories");

            modelBuilder.Entity<ShopInterface>()
                .ToTable("ShopInterfaces");
        }
    }
}
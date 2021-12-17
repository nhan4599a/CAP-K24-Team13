using DatabaseAccessor.Repositories;
using DatabaseSharing;
using Shared.RequestModels;
using System;
using Xunit;
using DatabaseAccessor.Models;

namespace TestShopProductService
{
    public class TestProductRepository
    {
        private readonly ProductRepository _repository;

        public TestProductRepository()
        {
            FakeApplicationDbContext dbContext = new();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            if (dbContext.ShopCategories.Find(1) == null)
            {
                dbContext.ShopCategories.Add(new ShopCategory
                {
                    Id = 1,
                    CategoryName = "Demo",
                    ShopId = 0,
                    IsDisabled = false,
                    Special = 0
                });
                dbContext.SaveChanges();
            }
            _repository = new ProductRepository(new FakeApplicationDbContext(), null);
        }

        [Fact]
        public async void TestAddProductSuccess()
        {
            var requestModel = new CreateOrEditProductRequestModel
            {
                CategoryId = 1,
                Description = "description for product",
                Discount = 0,
                ProductName = "demo product",
                ImagePaths = Array.Empty<string>(),
                Quantity = 1,
            };

            var result = await _repository.AddProductAsync(requestModel);

            Assert.NotNull(result);
            Assert.True(result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
        }
    }
}
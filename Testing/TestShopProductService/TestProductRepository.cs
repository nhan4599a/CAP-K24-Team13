using DatabaseAccessor.Repositories;
using DatabaseSharing;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using System;
using System.Linq;
using UnitTestSupport;
using Xunit;

namespace TestShopProductService
{
    [TestCaseOrderer("UnitTestSupport.PriorityTestCaseOrderer", "UnitTestSupport")]
    public class TestProductRepository
    {
        private readonly ProductRepository _repository;

        public TestProductRepository()
        {
            FakeApplicationDbContext dbContext = new();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            _repository = new ProductRepository(dbContext, null);
        }

        [TestCasePriority(1)]
        [Fact]
        public async void TestAddProductSuccess()
        {
            var requestModel = new CreateProductRequestModel
            {
                CategoryId = 1,
                Description = "description for product",
                Discount = 0,
                ProductName = "demo product",
                Price = 24000,
                ImagePaths = Array.Empty<string>(),
                Quantity = 1,
                ShopId = 1
            };

            var result = await _repository.AddProductAsync(requestModel);
            Assert.NotNull(result);
            Assert.IsType<CommandResponse<Guid>>(result);
            Assert.True(result.IsSuccess);
            Assert.Null(result.Exception);
            Assert.Null(result.ErrorMessage);
            Assert.NotEqual(Guid.Empty, result.Response);
        }

        [TestCasePriority(2)]
        [Fact]
        public async void TestAddProductFailedBecauseShopIsDisabled()
        {
            var requestModel = new CreateProductRequestModel
            {
                CategoryId = 1,
                Description = "description for product",
                Discount = 0,
                ProductName = "demo product",
                Price = 24000,
                ImagePaths = Array.Empty<string>(),
                Quantity = 1,
                ShopId = 2
            };

            var result = await _repository.AddProductAsync(requestModel);
            Assert.NotNull(result);
            Assert.IsType<CommandResponse<Guid>>(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Null(result.Exception);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Shop is already disabled!", result.ErrorMessage);
        }

        [TestCasePriority(3)]
        [Fact]
        public async void TestEditProductSuccess()
        {
            var product = FakeApplicationDbContext.listProducts.First();
            var requestModel = new EditProductRequestModel
            {
                ProductName = product.ProductName,
                Description = product.Description,
                Discount = product.Discount,
                Price = product.Price,
                CategoryId = 1,
            };

            var result = await _repository.EditProductAsync(product.Id, requestModel);

            Assert.NotNull(result);

            Assert.IsType<CommandResponse<ProductDTO>>(result);
            Assert.True(result.IsSuccess);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
        }
        
        [TestCasePriority(4)]
        [Fact]
        public async void TestEditProductFailBecauseProductNotFound()
        {
            var requestModel = new EditProductRequestModel
            {
                ProductName = "Demo product",
                CategoryId = 1,
                Description = "Demo description",
                Discount = 0,
                ImagePaths = Array.Empty<string>(),
                Price = 24000
            };

            var result = await _repository.EditProductAsync(Guid.Empty, requestModel);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is not found", result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(5)]
        [Fact]
        public async void TestEditProductFailBecauseProductIsDisabled()
        {
            var requestModel = new EditProductRequestModel
            {
                ProductName = "Demo product",
                CategoryId = 1,
                Description = "Demo description",
                Discount = 0,
                ImagePaths = Array.Empty<string>(),
                Price = 24000
            };

            var result = await _repository.EditProductAsync(FakeApplicationDbContext.listProducts.Last().Id, requestModel);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is disabled", result.ErrorMessage);
            Assert.Null(result.Exception);
        }


        [TestCasePriority(6)]
        [Fact]
        public async void TestDeactivateProductSuccess()
        {
            var product = FakeApplicationDbContext.listProducts.First();

            var result = await _repository.ActivateProductAsync(product.Id, false);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.False(result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(7)]
        [Fact]
        public async void TestActivateProductSuccess()
        {
            var product = FakeApplicationDbContext.listProducts.Last();

            var result = await _repository.ActivateProductAsync(product.Id, true);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(8)]
        [Fact]
        public async void TestDeactivateProductFailedBecauseProductNotFound()
        {
            var result = await _repository.ActivateProductAsync(Guid.Empty, false);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is not found", result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(9)]
        [Fact]
        public async void TestActivateProductFailedBecauseProductNotFound()
        {
            var result = await _repository.ActivateProductAsync(Guid.Empty, true);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is not found", result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(10)]
        [Fact]
        public async void TestDeactivateProductFailedBecauseProductIsDeactivated()
        {
            var product = FakeApplicationDbContext.listProducts.Last();

            var result = await _repository.ActivateProductAsync(product.Id, false);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is already deactivated", result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(11)]
        [Fact]
        public async void TestActivateProductFailedBecauseProductIsActivated()
        {
            var product = FakeApplicationDbContext.listProducts.First();

            var result = await _repository.ActivateProductAsync(product.Id, true);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is already activated", result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(12)]
        [Fact]
        public async void TestGetProduct()
        {
            var product = FakeApplicationDbContext.listProducts.First();

            var result = await _repository.GetProductAsync(product.Id);

            Assert.NotNull(result);
            Assert.Equal(product.Id, Guid.Parse(result.Id));
            Assert.Equal(product.ProductName, result.ProductName);
            Assert.Equal(product.Quantity, result.Quantity);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.CategoryId, result.CategoryId);
            Assert.Equal(product.Category, result.CategoryName);
            Assert.Equal(product.Description, result.Description);
        }

        [TestCasePriority(13)]
        [Fact]
        public async void TestGetProductNull()
        {
            var product = await _repository.GetProductAsync(Guid.Empty);

            Assert.Null(product);
        }

        [TestCasePriority(14)]
        [Fact]
        public async void TestFindProducts()
        {
            var products = await _repository.FindProductsAsync("", new PaginationInfo
            {
                PageNumber = 1,
                PageSize = 10
            });

            Assert.NotNull(products);
            Assert.Equal(1, products.PageNumber);
            Assert.Equal(10, products.PageSize);
        }
    }
}
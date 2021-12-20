using DatabaseAccessor.Models;
using DatabaseAccessor.Repositories;
using DatabaseAccessor.Repositories.Interfaces;
using DatabaseSharing;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.RequestModels;
using ShopProductService.Controllers;
using System;
using UnitTestSupport;
using Xunit;




namespace TestShopProductService
{
    [TestCaseOrderer("UnitTestSupport.PriorityTestCaseOrderer", "UnitTestSupport")]


    public class TestProductRepository
    {
        ShopProduct service;
        Mock<IProductRepository> repositoryMock;
        Mock<ShopProduct> productMock;
        private readonly ProductRepository _repository;

        public TestProductRepository()
        {
            FakeApplicationDbContext dbContext = new();
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
            }
            if (dbContext.ShopCategories.Find(2) == null)
            {
                dbContext.ShopCategories.Add(new ShopCategory
                {
                    Id = 2,
                    CategoryName = "disabled demo",
                    ShopId = 0,
                    IsDisabled = true,
                    Special = 0
                });
            }
            dbContext.SaveChanges();
            _repository = new ProductRepository(dbContext, null);
        }

        [TestCasePriority(1)]
        [Fact]
        public async void TestAddProductSuccess()
        {
            var productCount = (await _repository.GetAllProductAsync()).Count;
            var requestModel = new CreateOrEditProductRequestModel
            {
                CategoryId = 1,
                Description = "description for product",
                Discount = 0,
                ProductName = "demo product",
                Price = 24000,
                ImagePaths = Array.Empty<string>(),
                Quantity = 1
            };

            var result = await _repository.AddProductAsync(requestModel);

            var afterProductCount = (await _repository.GetAllProductAsync()).Count;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
            Assert.Equal(productCount + 1, afterProductCount);
        }

        [TestCasePriority(2)]
        [Fact]
        public async void TestAddProductFailBecauseCategoryNotExisted()
        {
            var productCount = (await _repository.GetAllProductAsync()).Count;
            var requestModel = new CreateOrEditProductRequestModel
            {
                CategoryId = 0,
                Description = "Demo description",
                ProductName = "Demo product",
                Discount = 0,
                Quantity = 1,
                ImagePaths = Array.Empty<string>(),
                Price = 24000
            };

            var result = await _repository.AddProductAsync(requestModel);

            var afterProductCount = (await _repository.GetAllProductAsync()).Count;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Category is not found", result.ErrorMessage);
            Assert.Null(result.Exception);
            Assert.Equal(productCount, afterProductCount);
        }

        [TestCasePriority(3)]
        [Fact]
        public async void TestAddProductFailBecauseCategoryIsDisabled()
        {
            var productCount = (await _repository.GetAllProductAsync()).Count;
            var requestModel = new CreateOrEditProductRequestModel
            {
                CategoryId = 2,
                Description = "Demo description",
                ProductName = "Demo product",
                Discount = 0,
                Quantity = 1,
                ImagePaths = Array.Empty<string>(),
                Price = 24000
            };

            var result = await _repository.AddProductAsync(requestModel);
            var afterProductCount = (await _repository.GetAllProductAsync()).Count;

            Assert.NotNull(result);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.NotNull(result.ErrorMessage);
            Assert.Equal("Category is disabled", result.ErrorMessage);
            Assert.Null(result.Exception);
            Assert.Equal(productCount, afterProductCount);
        }

        [TestCasePriority(4)]
        [Fact]
        public async void TestEditProductSuccess()
        {
            var product = (await _repository.GetAllProductAsync())[0];
            var requestModel = new CreateOrEditProductRequestModel
            {
                ProductName = product.ProductName,
                Description = product.Description,
                Discount = product.Discount,
                Price = product.Price,
                CategoryId = 1,
                Quantity = 2
            };

            var result = await _repository.EditProductAsync(Guid.Parse(product.Id), requestModel);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
        }
        
        [TestCasePriority(5)]
        [Fact]
        public async void TestEditProductFailBecauseProductNotFound()
        {
            var requestModel = new CreateOrEditProductRequestModel
            {
                ProductName = "Demo product",
                CategoryId = 1,
                Description = "Demo description",
                Discount = 0,
                ImagePaths = Array.Empty<string>(),
                Price = 24000,
                Quantity = 1
            };

            var result = await _repository.EditProductAsync(Guid.Empty, requestModel);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is not found", result.ErrorMessage);
            Assert.Null(result.Exception);
        }

        [TestCasePriority(6)]
        [Fact]
        public async void TestDeactivateProductSuccess()
        {
            var product = (await _repository.GetAllProductAsync())[0];

            var result = await _repository.ActivateProductAsync(Guid.Parse(product.Id), false);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.False(result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);

            var afterProduct = (await _repository.GetAllProductAsync())[0];

            Assert.True(afterProduct.IsDisabled);
        }

        [TestCasePriority(7)]
        [Fact]
        public async void TestActivateProductSuccess()
        {
            var product = (await _repository.GetAllProductAsync())[0];
            var result = await _repository.ActivateProductAsync(Guid.Parse(product.Id), true);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.True(result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);

            var afterProduct = (await _repository.GetAllProductAsync())[0];

            Assert.False(afterProduct.IsDisabled);
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
            var product = (await _repository.GetAllProductAsync())[0];

            var result = await _repository.ActivateProductAsync(Guid.Parse(product.Id), false);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is already deactivated", result.ErrorMessage);
            Assert.Null(result.Exception);

            var afterProduct = (await _repository.GetAllProductAsync())[0];

            Assert.Equal(product.IsDisabled, afterProduct.IsDisabled);
        }

        [TestCasePriority(11)]
        [Fact]
        public async void TestActivateProductFailedBecauseProductIsActivated()
        {
            var product = (await _repository.GetAllProductAsync())[0];

            var result = await _repository.ActivateProductAsync(Guid.Parse(product.Id), true);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Throws<InvalidOperationException>(() => result.Response);
            Assert.Equal("Product is already activated", result.ErrorMessage);
            Assert.Null(result.Exception);

            var afterProduct = (await _repository.GetAllProductAsync())[0];

            Assert.Equal(product.IsDisabled, afterProduct.IsDisabled);
        }

        [TestCasePriority(12)]
        [Fact]
        public async void TestGetProduct()
        {
            var firstProductId = (await _repository.GetAllProductAsync())[0].Id;

            var product = await _repository.GetProductAsync(Guid.Parse(firstProductId));

            Assert.NotNull(product);
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
        public void TestNotSaveValidProduct()
        {
            productMock.SetupGet(p => p.IsValid).Returns(false);
            bool result = service.save(productMock.Object);

            Assert.False(result);
            repositoryMock.Verify(r => r.Save(It.IsAny<Product>()), Times.Never());
        }
        [TestCasePriority(15)]
        [Fact]
        public void TestSaveValidProduct()
        {
            productMock.SetupGet(p => p.IsValid).Returns(true);
            repositoryMock.Setup(r => r.Save(productMock.Object)).Returns(true);

            bool result = service.save(productMock.Object);

            Assert.True(result);
            repositoryMock.VerifyAll();
        }
        [TestCasePriority(16)]
        [Fact]]
        public void Test_SaveCalledWhenCreateCalled()
        {
            
            var mockRepository = new Mock<IProductRepository>();
            ProductController controller = new ProductController(mockRepository);

           
            ActionResult actionResult = controller.Create(user);

           
            mockRepository.Verify(rep => rep.Save(), Times.AtLeastOnce());
        }
    }
}
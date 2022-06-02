using DatabaseAccessor.Repositories.Abstraction;
using Moq;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using ShopProductService.Commands.Product;
using ShopProductService.Handlers.Product;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace TestShopProductService
{
    public class TestProductHandlers
    {
        [Fact]
        public async void TestActivateProductCommandHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.ActivateProductAsync(It.IsAny<Guid>(), It.IsAny<bool>()))
                .ReturnsAsync(CommandResponse<bool>.Success(true));

            using var handler = new ActivateProductCommandHandler(mockProductRepository.Object);

            var command = new ActivateProductCommand
            {
                Id = Guid.NewGuid(),
                IsActivateCommand = true
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestCreateProductCommandHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.AddProductAsync(It.IsAny<CreateProductRequestModel>()))
                .ReturnsAsync(CommandResponse<Guid>.Success(Guid.NewGuid()));

            using var handler = new CreateProductCommandHandler(mockProductRepository.Object);

            var command = new CreateProductCommand
            {
                RequestModel = new CreateProductRequestModel(),
                ShopId = 1
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestEditProductCommandHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.EditProductAsync(It.IsAny<Guid>(), It.IsAny<EditProductRequestModel>()))
                .ReturnsAsync(CommandResponse<ProductDTO>.Success(new ProductDTO()));

            using var handler = new EditProductCommandHandler(mockProductRepository.Object);

            var command = new EditProductCommand
            {
                RequestModel = new EditProductRequestModel(),
                Id = Guid.NewGuid()
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindAllProductQueryHandler()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetAllProductAsync(It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindAllProductQueryHandler(mockProductRepository.Object);

            var command = new FindAllProductsQuery
            {
                IncludeFilter = false,
                PaginationInfo = PaginationInfo.Default
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindBestSellerProductsQueryHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetBestSellerProductsAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<MinimalProductDTO>());

            using var handler = new FindBestSellerProductsQueryHandler(mockProductRepository.Object);

            var command = new FindBestSellerProductsQuery
            {
                ShopId = 1
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductByIdQueryHandlerMinimal()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetMinimalProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new MinimalProductDTO());

            using var handler = new FindProductByIdQueryHandler(mockProductRepository.Object);

            var command = new FindProductByIdQuery
            {
                Id = Guid.NewGuid(),
                IsMinimal = true
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductByIdQueryHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetProductAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new ProductWithCommentsDTO());

            using var handler = new FindProductByIdQueryHandler(mockProductRepository.Object);

            var command = new FindProductByIdQuery
            {
                Id = Guid.NewGuid(),
                IsMinimal = false
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsByCategoryIdQueryHandler()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetProductsOfCategoryAsync(It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OrderByDirection>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsByCategoryIdQueryHandler(mockProductRepository.Object);

            var command = new FindProductsByCategoryIdQuery
            {
                Keyword = null
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsByCategoryIdQueryHandlerTrim()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetProductsOfCategoryAsync(It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OrderByDirection>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsByCategoryIdQueryHandler(mockProductRepository.Object);

            var command = new FindProductsByCategoryIdQuery
            {
                Keyword = " abc "
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsByKeywordQueryHandler()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.FindProductsAsync(It.IsAny<string>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsByKeywordQueryHandler(mockProductRepository.Object);

            var command = new FindProductsByKeywordQuery
            {
                Keyword = " abc "
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsByShopIdAndKeywordQueryHandler()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.FindProductsOfShopAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<PaginationInfo>(), It.IsAny<bool>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsByShopIdAndKeywordQueryHandler(mockProductRepository.Object);

            var command = new FindProductsByShopIdAndKeywordQuery
            {
                Keyword = " abc ",
                ShopId = 1
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsByShopIdQueryHandler()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetAllProductsOfShopAsync(It.IsAny<int>(), It.IsAny<PaginationInfo>(), It.IsAny<bool>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsByShopIdQueryHandler(mockProductRepository.Object);

            var command = new FindProductsByShopIdQuery
            {
                ShopId = 1
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsOfShopInCategoryQueryHandler()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetProductsOfCategoryAsync(It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OrderByDirection>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsOfShopInCategoryQueryHandler(mockProductRepository.Object);

            var command = new FindProductsOfShopInCategoryQuery
            {
                ShopId = 1
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindProductsOfShopInCategoryQueryHandlerTrim()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetProductsOfCategoryAsync(It.IsAny<int?>(), It.IsAny<List<int>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<OrderByDirection>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            using var handler = new FindProductsOfShopInCategoryQueryHandler(mockProductRepository.Object);

            var command = new FindProductsOfShopInCategoryQuery
            {
                ShopId = 1,
                Keyword = " abc  "
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindRelatedProductsQueryHandler()
        {

            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetRelatedProductsAsync(It.IsAny<Guid>()))
                .ReturnsAsync(CommandResponse<List<ProductDTO>>.Success(new List<ProductDTO>()));

            using var handler = new FindRelatedProductsQueryHandler(mockProductRepository.Object);

            var command = new FindRelatedProductsQuery
            {
                Id = Guid.NewGuid()
            };

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestGetMostSaleOffProductsQueryHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetTopMostSaleOffProductsAsync())
                .ReturnsAsync(new List<MinimalProductDTO>());

            using var handler = new GetMostSaleOffProductsQueryHandler(mockProductRepository.Object);

            var command = new GetMostSaleOffProductsQuery();

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestGetTopNewProductsQueryHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.GetTopNewsProductsAsync())
                .ReturnsAsync(new List<ProductDTO>());

            using var handler = new GetTopNewProductsQueryHandler(mockProductRepository.Object);

            var command = new GetTopNewProductsQuery();

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestImportProductCommandHandler()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository
                .Setup(e => e.ImportProductQuantityAsync(It.IsAny<Guid>(), It.IsAny<int>()))
                .ReturnsAsync(CommandResponse<int>.Success(1));

            using var handler = new ImportProductCommandHandler(mockProductRepository.Object);

            var command = new ImportProductCommand();

            var result = await handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
        }
    }
}

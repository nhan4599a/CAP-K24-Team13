using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using Shared.Validations;
using ShopProductService;
using ShopProductService.Commands.Product;
using ShopProductService.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using UnitTestSupport;
using Xunit;

namespace TestShopProductService
{
    [TestCaseOrderer("UnitTestSupport.PriorityTestCaseOrderer", "UnitTestSupport")]
    public class TestProductController
    {
        [TestCasePriority(1)]
        [Fact]
        public async void TestAddProductSuccess()
        {
            var fileStoreMock = new Mock<IFileStorable>();
            fileStoreMock
                .Setup(e => e.SaveFilesAsync(It.IsAny<IFormFileCollection>(), It.IsAny<bool>(),
                    It.IsAny<FileValidationRuleSet>()))
                .ReturnsAsync(Array.Empty<string>());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<CreateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<Guid>.Success(Guid.NewGuid()));

            var contextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            var formMock = new Mock<IFormCollection>();
            var formFileMock = new Mock<IFormFileCollection>();
            formMock.Setup(e => e.Files).Returns(formFileMock.Object);
            httpRequestMock.Setup(e => e.Form).Returns(formMock.Object);
            contextMock.Setup(e => e.Request).Returns(httpRequestMock.Object);

            var productController = new ProductController(mediatorMock.Object, fileStoreMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = contextMock.Object }
            };

            var requestModel = new CreateProductRequestModel
            {
                CategoryId = 1,
                Description = "Some description",
                Price = 24000,
                ProductName = "Some name",
                Quantity = 1,
                Discount = 0
            };

            var result = await productController.AddProduct(requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.Empty(result.ErrorMessage);
        }

        [TestCasePriority(2)]
        [Fact]
        public async void TestAddProductFail()
        {
            var imageManagerMock = new Mock<IFileStorable>();
            imageManagerMock
                .Setup(e => e.SaveFilesAsync(It.IsAny<IFormFileCollection>(),
                    It.IsAny<bool>(), It.IsAny<FileValidationRuleSet>()))
                .ReturnsAsync(Array.Empty<string>());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<CreateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<Guid>.Error("Action failed", null));

            var contextMock = new Mock<HttpContext>();
            var httpRequestMock = new Mock<HttpRequest>();
            var formMock = new Mock<IFormCollection>();
            var formFileMock = new Mock<IFormFileCollection>();
            formMock.Setup(e => e.Files).Returns(formFileMock.Object);
            httpRequestMock.Setup(e => e.Form).Returns(formMock.Object);
            contextMock.Setup(e => e.Request).Returns(httpRequestMock.Object);

            var controller = new ProductController(mediatorMock.Object, imageManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = contextMock.Object }
            };

            var requestModel = new CreateProductRequestModel
            {
                CategoryId = 1,
                Description = "Some description",
                Price = 24000,
                ProductName = "Some name",
                Quantity = 1,
                Discount = 0
            };

            var result = await controller.AddProduct(requestModel);

            Assert.NotNull(result);
            Assert.Equal(500, result.ResponseCode);
            Assert.Equal("Action failed", result.ErrorMessage);
        }

        [TestCasePriority(3)]
        [Fact]
        public async void TestEditProductSuccess()
        {
            var imageManagerMock = new Mock<IFileStorable>();

            imageManagerMock
                .Setup(e => e.EditFilesAsync(It.IsAny<string[]>(), It.IsAny<IFormFileCollection>(), It.IsAny<bool>(),
                    It.IsAny<FileValidationRuleSet>()))
                .ReturnsAsync(Array.Empty<string>());

            var emptyProductDTO = new ProductDTO();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<EditProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<ProductDTO>.Success(emptyProductDTO));

            var contextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();
            var formMock = new Mock<IFormCollection>();
            var formFileMock = new Mock<IFormFileCollection>();

            formMock.Setup(e => e.Files).Returns(formFileMock.Object);
            requestMock.Setup(e => e.Form).Returns(formMock.Object);
            contextMock.Setup(e => e.Request).Returns(requestMock.Object);

            var controller = new ProductController(mediatorMock.Object, imageManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = contextMock.Object }
            };

            var requestModel = new EditProductRequestModel
            {
                CategoryId = 1,
                Description = "Some description",
                Price = 24000,
                ProductName = "Some name",
                Discount = 0
            };

            var result = await controller.EditProduct(Guid.NewGuid().ToString(), requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.Empty(result.ErrorMessage);
        }

        [TestCasePriority(4)]
        [Fact]
        public async void TestEditProductFail()
        {
            var imageManagerMock = new Mock<IFileStorable>();

            imageManagerMock
                .Setup(e => e.EditFilesAsync(It.IsAny<string[]>(), It.IsAny<IFormFileCollection>(), It.IsAny<bool>(),
                    It.IsAny<FileValidationRuleSet>()))
                .ReturnsAsync(Array.Empty<string>());

            var emptyProductDTO = new ProductDTO();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<EditProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<ProductDTO>.Error("Action failed", null));

            var contextMock = new Mock<HttpContext>();
            var requestMock = new Mock<HttpRequest>();
            var formMock = new Mock<IFormCollection>();
            var formFileMock = new Mock<IFormFileCollection>();

            formMock.Setup(e => e.Files).Returns(formFileMock.Object);
            requestMock.Setup(e => e.Form).Returns(formMock.Object);
            contextMock.Setup(e => e.Request).Returns(requestMock.Object);

            var controller = new ProductController(mediatorMock.Object, imageManagerMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = contextMock.Object }
            };

            var requestModel = new EditProductRequestModel
            {
                CategoryId = 1,
                Description = "Some description",
                Price = 24000,
                ProductName = "Some name",
                Discount = 0
            };

            var result = await controller.EditProduct(Guid.NewGuid().ToString(), requestModel);

            Assert.NotNull(result);
            Assert.Equal(500, result.ResponseCode);
            Assert.NotEmpty(result.ErrorMessage);
            Assert.Equal("Action failed", result.ErrorMessage);
        }

        [TestCasePriority(5)]
        [Fact]
        public async void TestActivateProductSuccess()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<ActivateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Success(true));

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.DeleteProduct(Guid.NewGuid().ToString(), DeleteAction.Activate);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.Empty(result.ErrorMessage);
        }

        [TestCasePriority(6)]
        [Fact]
        public async void TestDeactivateProductSuccess()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<ActivateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Success(false));

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.DeleteProduct(Guid.NewGuid().ToString(), DeleteAction.Deactivate);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.Empty(result.ErrorMessage);
        }

        [TestCasePriority(7)]
        [Fact]
        public async void TestActivateProductFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<ActivateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Error("Action failed", null));

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.DeleteProduct(Guid.NewGuid().ToString(), DeleteAction.Activate);

            Assert.NotNull(result);
            Assert.Equal(500, result.ResponseCode);
            Assert.Equal("Action failed", result.ErrorMessage);
        }

        [TestCasePriority(8)]
        [Fact]
        public async void TestDeactivateProductFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<ActivateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Error("Action failed", null));

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.DeleteProduct(Guid.NewGuid().ToString(), DeleteAction.Deactivate);

            Assert.NotNull(result);
            Assert.Equal(500, result.ResponseCode);
            Assert.Equal("Action failed", result.ErrorMessage);
        }

        [TestCasePriority(9)]
        [Fact]
        public async void TestFindAllProductSuccess()
        {
            var paginatedList = new PaginatedList<ProductDTO>(new List<ProductDTO>(), 1, 10, 0);
            var requestModel = new SearchRequestModel { Keyword = string.Empty, PageSize = 5, PageNumber = 1 };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindAllProductsQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.FindProducts(requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
        }

        [TestCasePriority(10)]
        [Fact]
        public async void TestFindProductsByKeywordSuccess()
        {
            var paginatedList = new PaginatedList<ProductDTO>(new List<ProductDTO>(), 1, 10, 0);
            var requestModel = new SearchRequestModel { Keyword = "abc", PageSize = 5, PageNumber = 1 };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductsByKeywordQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.FindProducts(requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
        }

        [TestCasePriority(11)]
        [Fact]
        public async void TestGetSingleProductSuccess()
        {
            var id = Guid.NewGuid().ToString();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductByIdQuery>(), CancellationToken.None))
                .ReturnsAsync(new ProductWithCommentsDTO { Id = id });

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.GetSingleProduct(id);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.Empty(result.ErrorMessage);
            //Assert.NotNull(result.Data);
            //Assert.Equal(id, result.Data.Id);
        }

        [TestCasePriority(12)]
        [Fact]
        public async void TestGetSingleProductFailed()
        {
            var id = Guid.NewGuid().ToString();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductByIdQuery>(), CancellationToken.None))
                .ReturnsAsync((ProductWithCommentsDTO?)null);

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.GetSingleProduct(id);

            Assert.NotNull(result);
            Assert.Equal(404, result.ResponseCode);
            Assert.NotEmpty(result.ErrorMessage);
            Assert.Equal("Product is not found", result.ErrorMessage);
        }

        [TestCasePriority(13)]
        [Fact]
        public void TestGetImageSuccess()
        {
            var fileStoreMock = new Mock<IFileStorable>();

            var fileResponseMock = new Mock<FileResponse>();

            fileResponseMock.SetupGet(e => e.IsExisted).Returns(true);

            fileResponseMock.SetupGet(e => e.FullPath).Returns("abc.jpg");

            fileResponseMock.SetupGet(e => e.MimeType).Returns("image/jpg");

            fileStoreMock
                .Setup(e => e.GetFile(It.IsAny<string>()))
                .Returns(fileResponseMock.Object);

            var controller = new ProductController(null, fileStoreMock.Object);

            var result = controller.GetImage("abc.jpg");

            Assert.NotNull(result);
            Assert.IsType<PhysicalFileResult>(result);

            var temp = result as PhysicalFileResult;

            Assert.Equal("abc.jpg", temp?.FileName);
            Assert.Equal("image/jpg", temp?.ContentType);
        }

        [TestCasePriority(14)]
        [Fact]
        public void TestGetImageFailed()
        {
            var fileResponseMock = new Mock<FileResponse>();

            fileResponseMock.SetupGet(e => e.IsExisted).Returns(false);

            var fileStoreMock = new Mock<IFileStorable>();

            fileStoreMock
                .Setup(e => e.GetFile(It.IsAny<string>()))
                .Returns(fileResponseMock.Object);

            var controller = new ProductController(null, fileStoreMock.Object);

            var result = controller.GetImage("abc.jpg");

            Assert.NotNull(result);
            Assert.IsType<StatusCodeResult>(result);

            var temp = result as StatusCodeResult;

            Assert.Equal(StatusCodes.Status404NotFound, temp?.StatusCode);
        }

        [TestCasePriority(15)]
        [Fact]
        public async void TestGetProductsOfShop()
        {
            var mediatorMock = new Mock<IMediator>();
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductsByShopIdQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetProductsOfShop(0, new SearchRequestModel());

            Assert.NotNull(result);
            Assert.IsType<ApiResult<PaginatedList<ProductDTO>>>(result);
        }

        [TestCasePriority(16)]
        [Fact]
        public async void TestGetProductsOfShopAndKeyword()
        {
            var mediatorMock = new Mock<IMediator>();
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());

            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductsByShopIdAndKeywordQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetProductsOfShop(0, new SearchRequestModel
            {
                Keyword = "abc"
            });

            Assert.NotNull(result);
            Assert.IsType<ApiResult<PaginatedList<ProductDTO>>>(result);
        }

        [TestCasePriority(17)]
        [Fact]
        public async void TestGetMinimalSingleProductSuccess()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductByIdQuery>(), CancellationToken.None))
                .ReturnsAsync(new MinimalProductDTO());

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetMinimalSingleProduct(Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult<MinimalProductDTO>>(result);
        }

        [TestCasePriority(18)]
        [Fact]
        public async void TestGetMinimalSingleProductFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductByIdQuery>(), CancellationToken.None))
                .ReturnsAsync((MinimalProductDTO?)null);

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetMinimalSingleProduct(Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult>(result);
            Assert.Equal(404, result.ResponseCode);
            Assert.Equal("Product is not found", result.ErrorMessage);
        }

        [TestCasePriority(19)]
        [Fact]
        public async void TestGetRelatedProductSuccess()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindRelatedProductsQuery>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<List<ProductDTO>>.Success(new List<ProductDTO>()));

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetRelatedProducts(Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult<List<ProductDTO>>>(result);
        }

        [TestCasePriority(20)]
        [Fact]
        public async void TestGetRelatedProductFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindRelatedProductsQuery>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<List<ProductDTO>>.Error("", null));
            
            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetRelatedProducts(Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult>(result);
            Assert.Equal(404, result.ResponseCode);
        }

        [TestCasePriority(21)]
        [Fact]
        public async void TestImportProductSuccess()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<ImportProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<int>.Success(1));

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.ImportProduct(Guid.NewGuid().ToString(), 1);
            Assert.NotNull(result);
            Assert.IsType<ApiResult<int>>(result);
        }

        [TestCasePriority(22)]
        [Fact]
        public async void TestImportProductFailed()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<ImportProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<int>.Error("", null));

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.ImportProduct(Guid.NewGuid().ToString(), 1);
            Assert.NotNull(result);
            Assert.IsType<ApiResult>(result);
            Assert.Equal(500, result.ResponseCode);
        }

        [TestCasePriority(23)]
        [Fact]
        public async void TestGetBestSellerProducts()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindBestSellerProductsQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<MinimalProductDTO>());

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetBestSellerProducts(1);
            Assert.NotNull(result);
            Assert.IsType<ApiResult<List<MinimalProductDTO>>>(result);
        }

        [TestCasePriority(24)]
        [Fact]
        public async void TestGetProductOfCategory()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductsByCategoryIdQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetProductsOfCategory(new List<int> { 1 }, new SearchRequestModel(), OrderByDirection.Unspecified);
            Assert.NotNull(result);
            Assert.IsType<ApiResult<PaginatedList<ProductDTO>>>(result);
        }

        [TestCasePriority(25)]
        [Fact]
        public async void TestGetProductsOfShopInCategory()
        {
            var paginatedList = new PaginatedList<ProductDTO>(1, 10, 1, new List<ProductDTO>());
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductsOfShopInCategoryQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetProductsOfShopInCategory(0, new List<int> { 1 }, new SearchRequestModel(), OrderByDirection.Unspecified);
            Assert.NotNull(result);
            Assert.IsType<ApiResult<PaginatedList<ProductDTO>>>(result);
        }

        [TestCasePriority(26)]
        [Fact]
        public async void TestGetTopMostSaleOffProducts()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<GetMostSaleOffProductsQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<MinimalProductDTO>());

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetTopMostSaleOffProducts();
            Assert.NotNull(result);
            Assert.IsType<ApiResult<List<MinimalProductDTO>>>(result);
        }

        [TestCasePriority(27)]
        [Fact]
        public async void TestGetTopMostNewProducts()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<GetTopNewProductsQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<ProductDTO>());

            var controller = new ProductController(mediatorMock.Object, null);
            var result = await controller.GetTopNewProducts();
            Assert.NotNull(result);
            Assert.IsType<ApiResult<List<ProductDTO>>>(result);
        }
    }
}
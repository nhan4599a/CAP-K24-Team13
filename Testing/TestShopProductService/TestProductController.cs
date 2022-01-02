using AspNetCoreSharedComponent.FileValidations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared;
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

            var requestModel = new CreateOrEditProductRequestModel
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
            Assert.NotEqual(Guid.Empty, result.Data);
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

            var requestModel = new CreateOrEditProductRequestModel
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
            Assert.Equal(Guid.Empty, result.Data);
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

            var requestModel = new CreateOrEditProductRequestModel
            {
                CategoryId = 1,
                Description = "Some description",
                Price = 24000,
                ProductName = "Some name",
                Quantity = 1,
                Discount = 0
            };

            var result = await controller.EditProduct(Guid.NewGuid().ToString(), requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.NotNull(result.Data);
            Assert.Equal(emptyProductDTO, result.Data);
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

            var requestModel = new CreateOrEditProductRequestModel
            {
                CategoryId = 1,
                Description = "Some description",
                Price = 24000,
                ProductName = "Some name",
                Quantity = 1,
                Discount = 0
            };

            var result = await controller.EditProduct(Guid.NewGuid().ToString(), requestModel);

            Assert.NotNull(result);
            Assert.Equal(500, result.ResponseCode);
            Assert.Null(result.Data);
            Assert.NotEqual(emptyProductDTO, result.Data);
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
            Assert.True(result.Data);
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
            Assert.False(result.Data);
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
            Assert.False(result.Data);
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
            Assert.False(result.Data);
            Assert.Equal("Action failed", result.ErrorMessage);
        }

        [TestCasePriority(9)]
        [Fact]
        public async void TestFindAllProductSuccess()
        {
            var paginatedList = new PaginatedList<ProductDTO>(new List<ProductDTO>(), 1, 10, 0);
            var requestModel = new SearchProductRequestModel { Keyword = string.Empty, PaginationInfo = new PaginationInfo() };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindAllProductQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.ListProduct(requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.PageNumber);
            Assert.Equal(1, result.Data.MaxPageNumber);
            Assert.False(result.Data.HasNextPage);
            Assert.False(result.Data.HasPreviousPage);
        }

        [TestCasePriority(10)]
        [Fact]
        public async void TestFindProductsByKeywordSuccess()
        {
            var paginatedList = new PaginatedList<ProductDTO>(new List<ProductDTO>(), 1, 10, 0);
            var requestModel = new SearchProductRequestModel { Keyword = "abc", PaginationInfo = new PaginationInfo() };
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductsByKeywordQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.ListProduct(requestModel);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.PageNumber);
            Assert.Equal(1, result.Data.MaxPageNumber);
            Assert.False(result.Data.HasNextPage);
            Assert.False(result.Data.HasPreviousPage);
        }

        [TestCasePriority(11)]
        [Fact]
        public async void TestGetSingleProductSuccess()
        {
            var id = Guid.NewGuid().ToString();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductByIdQuery>(), CancellationToken.None))
                .ReturnsAsync(new ProductDTO { Id = id });

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.GetSingleProduct(id);

            Assert.NotNull(result);
            Assert.Equal(200, result.ResponseCode);
            Assert.Empty(result.ErrorMessage);
            Assert.NotNull(result.Data);
            Assert.Equal(id, result.Data.Id);
        }

        [TestCasePriority(12)]
        [Fact]
        public async void TestGetSingleProductFailed()
        {
            var id = Guid.NewGuid().ToString();
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindProductByIdQuery>(), CancellationToken.None))
                .ReturnsAsync((ProductDTO?)null);

            var controller = new ProductController(mediatorMock.Object, null);

            var result = await controller.GetSingleProduct(id);

            Assert.NotNull(result);
            Assert.Equal(404, result.ResponseCode);
            Assert.NotEmpty(result.ErrorMessage);
            Assert.Equal("Product is not found", result.ErrorMessage);
            Assert.Null(result.Data);
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
    }
}

using AspNetCoreSharedComponent;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared;
using Shared.DTOs;
using Shared.RequestModels;
using Shared.Validations;
using ShopProductService;
using ShopProductService.Commands.Product;
using ShopProductService.Controllers;
using System;
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
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            var imageManagerMock = new Mock<ImageManager>(webHostEnvironmentMock.Object);
            imageManagerMock
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

            var productController = new ProductController(mediatorMock.Object, imageManagerMock.Object)
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
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            var imageManagerMock = new Mock<ImageManager>(webHostEnvironmentMock.Object);
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
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            var imageManagerMock = new Mock<ImageManager>(webHostEnvironmentMock.Object);

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
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            var imageManagerMock = new Mock<ImageManager>(webHostEnvironmentMock.Object);

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
    }
}

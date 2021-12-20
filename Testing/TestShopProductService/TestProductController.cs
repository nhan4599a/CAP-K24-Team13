using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared;
using Shared.RequestModels;
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
            var imageManagerMock = new Mock<ProductImageManager>(webHostEnvironmentMock.Object);
            imageManagerMock
                .Setup(e => e.SaveFilesAsync(It.IsAny<IFormFileCollection>()))
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
            Assert.Null(result.ErrorMessage);
        }

        [TestCasePriority(2)]
        [Fact]
        public async void TestAddProductFail()
        {
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            var imageManagerMock = new Mock<ProductImageManager>(mockWebHostEnvironment.Object);
            imageManagerMock
                .Setup(e => e.SaveFilesAsync(It.IsAny<IFormFileCollection>()))
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
            Assert.Equal(500, result.ResponseCode);
            Assert.Equal(Guid.Empty, result.Data);
            Assert.Equal("Action failed", result.ErrorMessage);
        }
    }
}

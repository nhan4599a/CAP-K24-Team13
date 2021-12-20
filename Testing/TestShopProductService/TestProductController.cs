using MediatR;
using Microsoft.AspNetCore.Http;
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
            var imageManagerMock = new Mock<ProductImageManager>();
            imageManagerMock
                .Setup(e => e.SaveFilesAsync(It.IsAny<IFormFileCollection>()))
                .ReturnsAsync(Array.Empty<string>());

            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<CreateProductCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<Guid>.Success(Guid.NewGuid()));

            var productController = new ProductController(mediatorMock.Object, imageManagerMock.Object);

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
        }
    }
}

using MediatR;
using Moq;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Category;
using ShopProductService.Controllers;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace TestShopProductService
{
    public class TestCategoryController
    {
        [Fact]
        public async void TestGetCategoriesOfShop()
        {
            var paginatedList = new PaginatedList<CategoryDTO>(1, 10, 1, new List<CategoryDTO>());
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindCategoriesByShopIdQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new CategoryController(mediatorMock.Object);
            var result = await controller.GetCategoriesOfShop(1, PaginationInfo.Default);

            Assert.NotNull(result);
            Assert.IsType<ApiResult<PaginatedList<CategoryDTO>>>(result);
        }

        [Fact]
        public async void TestGetCategories()
        {
            var paginatedList = new PaginatedList<CategoryDTO>(1, 10, 1, new List<CategoryDTO>());
            var mediatorMock = new Mock<IMediator>();
            mediatorMock
                .Setup(e => e.Send(It.IsAny<FindAllCategoriesQuery>(), CancellationToken.None))
                .ReturnsAsync(paginatedList);

            var controller = new CategoryController(mediatorMock.Object);
            var result = await controller.GetCategories(PaginationInfo.Default);

            Assert.NotNull(result);
            Assert.IsType<ApiResult<PaginatedList<CategoryDTO>>>(result);
        }
    }
}

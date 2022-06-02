using DatabaseAccessor.Repositories.Abstraction;
using Moq;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Category;
using ShopProductService.Handlers.Category;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace TestShopProductService
{
    public class TestCategoryHandlers
    {
        [Fact]
        public async void TestFindAllCategoriesQueryHandler()
        {
            var paginatedList = new PaginatedList<CategoryDTO>(1, 10, 1, new List<CategoryDTO>());
            var repositoryMock = new Mock<ICategoryRepository>();
            repositoryMock
                .Setup(e => e.GetCategoriesAsync(It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            var handler = new FindAllCategoriesQueryHandler(repositoryMock.Object);

            var model = new FindAllCategoriesQuery();
            var result = await handler.Handle(model, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestFindCategoriesByShopIdQueryHandler()
        {
            var paginatedList = new PaginatedList<CategoryDTO>(1, 10, 1, new List<CategoryDTO>());
            var repositoryMock = new Mock<ICategoryRepository>();
            repositoryMock
                .Setup(e => e.GetCategoriesOfShopAsync(It.IsAny<int>(), It.IsAny<PaginationInfo>()))
                .ReturnsAsync(paginatedList);

            var handler = new FindCategoriesByShopIdQueryHandler(repositoryMock.Object);

            var model = new FindCategoriesByShopIdQuery();
            var result = await handler.Handle(model, CancellationToken.None);
            Assert.NotNull(result);
        }
    }
}

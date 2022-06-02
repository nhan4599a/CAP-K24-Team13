using DatabaseAccessor.Repositories.Abstraction;
using Moq;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using ShopProductService.Commands.Cart;
using ShopProductService.Handlers.Cart;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace TestShopProductService
{
    public class TestCartHandlers
    {
        [Fact]
        public void TestAddCartItemCommandHandler()
        {
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock
               .Setup(e => e.AddProductToCartAsync(It.IsAny<AddOrEditQuantityCartItemRequestModel>()))
               .ReturnsAsync(CommandResponse<bool>.Success(true));

            var handler = new AddCartItemCommandHandler(repositoryMock.Object);

            var model = new AddCartItemCommand();

            var result = handler.Handle(model, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public void TestEditQuantityCartItemHandler()
        {
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock
               .Setup(e => e.EditQuantityAsync(It.IsAny<AddOrEditQuantityCartItemRequestModel>()))
               .ReturnsAsync(CommandResponse<bool>.Success(true));

            var handler = new EditQuantityCartItemHandler(repositoryMock.Object);

            var model = new EditQuantityCartItemCommand();

            var result = handler.Handle(model, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async void TestGetCartItemsHandler()
        {
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock
               .Setup(e => e.GetCartAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new List<CartItemDTO>());

            var handler = new GetCartItemsHandler(repositoryMock.Object);

            var model = new GetCartItemsQuery()
            {
                UserId = " asbasd "
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(model, CancellationToken.None));

            model.UserId = Guid.NewGuid().ToString();

            var result = await handler.Handle(model, CancellationToken.None);

            Assert.NotNull(result);
        }

        [Fact]
        public async void TestRemoveCartItemHandler()
        {
            var repositoryMock = new Mock<ICartRepository>();
            repositoryMock
               .Setup(e => e.RemoveCartItemAsync(It.IsAny<RemoveCartItemRequestModel>()))
               .ReturnsAsync(CommandResponse<bool>.Success(true));

            var handler = new RemoveCartItemHandler(repositoryMock.Object);

            var model = new RemoveCartItemCommand
            {
                requestModel = new RemoveCartItemRequestModel
                {
                    ProductId = Guid.NewGuid().ToString(),
                    UserId = Guid.NewGuid().ToString()
                }
            };

            var result = await handler.Handle(model, CancellationToken.None);

            Assert.NotNull(result);
        }
    }
}

using MediatR;
using Moq;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using ShopProductService.Commands.Cart;
using ShopProductService.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace TestShopProductService
{
    public class TestCartController
    {
        [Fact]
        public async void TestGetCartItems()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<GetCartItemsQuery>(), CancellationToken.None))
                .ReturnsAsync(new List<CartItemDTO>());

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.GetCartItems(Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult<List<CartItemDTO>>>(result);
        }

        [Fact]
        public async void TestAddCartItemSuccess()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<AddCartItemCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Success(true));

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.AddCartItem(new AddOrEditQuantityCartItemRequestModel());
            Assert.NotNull(result);
            Assert.IsType<ApiResult<bool>>(result);
        }

        [Fact]
        public async void TestAddCartItemFailed()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<AddCartItemCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Error("", null));

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.AddCartItem(new AddOrEditQuantityCartItemRequestModel());
            Assert.NotNull(result);
            Assert.IsType<ApiResult>(result);
            Assert.Equal(500, result.ResponseCode);
        }

        [Fact]
        public async void TestEditCartItemSuccess()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<EditQuantityCartItemCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Success(true));

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.EditQuantity(new AddOrEditQuantityCartItemRequestModel());
            Assert.NotNull(result);
            Assert.IsType<ApiResult<bool>>(result);
        }

        [Fact]
        public async void TestEditCartItemFailed()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<EditQuantityCartItemCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Error("", null));

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.EditQuantity(new AddOrEditQuantityCartItemRequestModel());
            Assert.NotNull(result);
            Assert.IsType<ApiResult>(result);
            Assert.Equal(500, result.ResponseCode);
        }

        [Fact]
        public async void TestDeleteCartItemSuccess()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<RemoveCartItemCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Success(true));

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.RemoveCartItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult<bool>>(result);
        }

        [Fact]
        public async void TestDeleteCartItemFailed()
        {
            var mediatorMock = new Mock<IMediator>();

            mediatorMock
                .Setup(e => e.Send(It.IsAny<RemoveCartItemCommand>(), CancellationToken.None))
                .ReturnsAsync(CommandResponse<bool>.Error("", null));

            var controller = new CartController(mediatorMock.Object);
            var result = await controller.RemoveCartItem(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.NotNull(result);
            Assert.IsType<ApiResult>(result);
            Assert.Equal(500, result.ResponseCode);
        }
    }
}

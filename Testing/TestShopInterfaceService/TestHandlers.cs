using DatabaseAccessor.Repositories.Abstraction;
using Moq;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
using ShopInterfaceService.Commands;
using ShopInterfaceService.Handlers;
using Xunit;

namespace TestShopInterfaceService
{
    public class TestHandlers
    {
        [Fact]
        public async void TestGetShopInterfacesQueryHandler()
        {
            var repositoryMock = new Mock<IShopInterfaceRepository>();
            repositoryMock
                .Setup(e => e.GetShopInterfacesAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new Dictionary<int, Shared.DTOs.ShopInterfaceDTO>());

            var handler = new GetShopInterfacesQueryHandler(repositoryMock.Object);

            var query = new GetShopInterfacesQuery();

            var result = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
        }

        [Fact]
        public async void TestGetShopAvatarQueryHandler()
        {
            var repositoryMock = new Mock<IShopInterfaceRepository>();
            repositoryMock
                .Setup(e => e.GetShopAvatar(It.IsAny<int[]>()))
                .ReturnsAsync(new Dictionary<int, string>());

            var handler = new GetShopAvatarQueryHandler(repositoryMock.Object);

            var query = new GetShopAvatarQuery();

            var result = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            handler.Dispose();
        }

        [Fact]
        public async void TestFindShopInterfaceByShopIdQueryHandler()
        {
            var repositoryMock = new Mock<IShopInterfaceRepository>();
            repositoryMock
                .Setup(e => e.FindShopInterfaceByShopIdAsync(It.IsAny<int>()))
                .ReturnsAsync(CommandResponse<ShopInterfaceDTO>.Success(new ShopInterfaceDTO()));

            var handler = new FindShopInterfaceByShopIdQueryHandler(repositoryMock.Object);

            var query = new FindShopInterfaceByShopIdQuery();

            var result = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            handler.Dispose();
        }

        [Fact]
        public async void TestCreateOrEditShopInterfaceCommandHandler()
        {
            var repositoryMock = new Mock<IShopInterfaceRepository>();
            repositoryMock
                .Setup(e => e.EditShopInterfaceAsync(It.IsAny<int>(), It.IsAny<CreateOrEditInterfaceRequestModel>()))
                .ReturnsAsync(CommandResponse<ShopInterfaceDTO>.Success(new ShopInterfaceDTO()));

            var handler = new CreateOrEditShopInterfaceCommandHandler(repositoryMock.Object);

            var query = new CreateOrEditShopInterfaceCommand();

            var result = await handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            handler.Dispose();
        }
    }
}
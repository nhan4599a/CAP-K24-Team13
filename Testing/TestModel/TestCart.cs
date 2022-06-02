using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestCart
    {
        [Fact]
        public void Test()
        {
            var guid = Guid.NewGuid();
            var model = new Cart
            {
                Id = 123,
                UserId = guid,
                Details = new List<CartDetail>(),
                User = new User()
            };
            Assert.Equal(123, model.Id);
            Assert.Equal(guid, model.UserId);
            Assert.Empty(model.Details);
            Assert.NotNull(model.User);
        }
    }
}
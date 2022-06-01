using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestCart
    {
        [Fact]
        public void Test()
        {
            var model = new Cart
            {
                Id = 123,
            };
            Assert.Equal(123, model.Id);
        }
    }
}

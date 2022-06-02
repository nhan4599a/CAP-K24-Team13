using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestInvoiceItemDTO
    {
        [Fact]
        public void Test()
        {
            var model = new InvoiceItemDTO
            {
                ProductId = "abc",
                Image = "abc",
                ProductName = "abc",
                Price = 123,
                Quantity = 123,
                CanBeRating =  true,
            };
            Assert.True(model.CanBeRating);
            Assert.Equal("abc", model.ProductId);
            Assert.Equal("abc", model.Image);
            Assert.Equal("abc", model.ProductName);
            Assert.Equal(123, model.Price);
            Assert.Equal(123, model.Quantity);

        }
    }
}

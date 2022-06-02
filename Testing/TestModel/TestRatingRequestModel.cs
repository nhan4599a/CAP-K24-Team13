using Shared.RequestModels;
using Xunit;

namespace TestModel
{
    public class TestRatingRequestModel
    {
        [Fact]
        public void Test()
        {
            var model = new RatingRequestModel
            {
                InvoiceId = 123,
                ProductId = "abc",
                Message = "abc",
                Star = 123
            };
            Assert.Equal(123, model.InvoiceId);
            Assert.Equal("abc", model.ProductId);
            Assert.Equal("abc", model.Message);
            Assert.Equal(123, model.Star);
        }
    }
}

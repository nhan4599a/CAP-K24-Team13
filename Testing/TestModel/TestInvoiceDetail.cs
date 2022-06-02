using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestInvoiceDetail
    {
        [Fact]
        public void Test()
        {
            var model = new InvoiceDetail
            {
                Id = 123,
                InvoiceId = 123,
                Quantity = 123,
                Price = 123,
                IsRated = true,
            };
            Assert.Equal(123, model.Id);
            Assert.Equal(123, model.InvoiceId);
            Assert.Equal(123, model.Quantity);
            Assert.Equal(123, model.Price);
            Assert.True(model.IsRated);
        }
    }
}

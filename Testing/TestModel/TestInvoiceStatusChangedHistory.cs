using DatabaseAccessor.Models;
using Xunit;


namespace TestModel
{
    public class TestInvoiceStatusChangedHistory
    {
        [Fact]
        public void Test()
        {
            var model = new InvoiceStatusChangedHistory
            {
                Id = 123,
                InvoiceId = 123
            };
            Assert.Equal(123, model.Id);
            Assert.Equal(123, model.InvoiceId);
        }
    }
}

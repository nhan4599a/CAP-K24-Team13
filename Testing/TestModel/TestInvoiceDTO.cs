using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestInvoiceDTO
    {
        [Fact]
        public void Test()
        {
            var model = new InvoiceDTO
            {
                InvoiceId = 123,
                InvoiceCode = "abc",
                RefId = "abc",
                ReceiverName = "abc",
                PhoneNumber = "abc",
                ShippingAddress = "abc",
                ShopId = 123,
                IsPaid = true
            };
            Assert.True(model.IsPaid);
            Assert.Equal(123, model.InvoiceId);
            Assert.Equal("abc", model.InvoiceCode);
            Assert.Equal("abc", model.RefId);
            Assert.Equal("abc", model.ReceiverName);
            Assert.Equal("abc", model.PhoneNumber);
            Assert.Equal("abc", model.ShippingAddress);
            Assert.Equal(123, model.ShopId);

        }
    }
}

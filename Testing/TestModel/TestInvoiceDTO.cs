using Shared.DTOs;
using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestInvoiceDTO
    {
        [Fact]
        public void Test()
        {
            var dateTime = DateTime.Now;
            var model = new InvoiceDTO
            {
                InvoiceId = 123,
                InvoiceCode = "abc",
                RefId = "abc",
                ReceiverName = "abc",
                PhoneNumber = "abc",
                ShippingAddress = "abc",
                ShopId = 123,
                IsPaid = true,
                CreatedAt = dateTime,
                PaymentMethod = PaymentMethod.MoMo,
                Status = InvoiceStatus.New
            };
            Assert.True(model.IsPaid);
            Assert.Equal(123, model.InvoiceId);
            Assert.Equal("abc", model.InvoiceCode);
            Assert.Equal("abc", model.RefId);
            Assert.Equal("abc", model.ReceiverName);
            Assert.Equal("abc", model.PhoneNumber);
            Assert.Equal("abc", model.ShippingAddress);
            Assert.Equal(123, model.ShopId);
            Assert.Equal(dateTime, model.CreatedAt);
            Assert.Equal(PaymentMethod.MoMo, model.PaymentMethod);
            Assert.Equal(InvoiceStatus.New, model.Status);
        }
    }
}

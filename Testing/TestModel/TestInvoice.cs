using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestInvoice
    {
        [Fact]
        public void Test()
        {
            var model = new Invoice
            {
                Id = 123,
                InvoiceCode = "abc",
                ShippingAddress = "abc",
                FullName = "abc",
                Phone = "123",
                Note = "abc",
                ShopId = 123,
                Status = Shared.Models.InvoiceStatus.New,
                PaymentMethod = Shared.Models.PaymentMethod.VISA,
                IsPaid = true,
                RefId = "abc"
            };
            Assert.Equal(123, model.Id);
            Assert.Equal("abc", model.InvoiceCode);
            Assert.Equal("abc", model.ShippingAddress);
            Assert.Equal("abc", model.FullName);
            Assert.Equal("123", model.Phone);
            Assert.Equal("abc", model.Note);
            Assert.Equal(123, model.ShopId);
            Assert.Equal(Shared.Models.InvoiceStatus.New, model.Status);
            Assert.Equal(Shared.Models.PaymentMethod.VISA, model.PaymentMethod);
            Assert.Equal(true, model.IsPaid);
            Assert.Equal("abc", model.RefId);




        }
    }
}

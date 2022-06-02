using DatabaseAccessor.Models;
using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestInvoiceStatusChangedHistory
    {
        [Fact]
        public void Test()
        {
            var dateTime = DateTime.Now;
            var model = new InvoiceStatusChangedHistory
            {
                Id = 123,
                InvoiceId = 123,
                ChangedDate = dateTime,
                Invoice = new Invoice(),
                NewStatus = InvoiceStatus.New,
                OldStatus = InvoiceStatus.Confirmed
            };
            Assert.Equal(123, model.Id);
            Assert.Equal(123, model.InvoiceId);
            Assert.Equal(dateTime, model.ChangedDate);
            Assert.NotNull(model.Invoice);
            Assert.Equal(InvoiceStatus.New, model.NewStatus);
            Assert.Equal(InvoiceStatus.Confirmed, model.OldStatus);
        }
    }
}

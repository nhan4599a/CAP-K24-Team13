using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestInvoiceStatusExtension
    {
        [Fact]
        public void Test()
        {
            var newDescription = InvoiceStatus.New.GetDescription();
            var confirmedDescription = InvoiceStatus.Confirmed.GetDescription();
            var receivedDescription = InvoiceStatus.ShipperReceived.GetDescription();
            var succeedDescription = InvoiceStatus.Succeed.GetDescription();
            var canceledDescription = InvoiceStatus.Canceled.GetDescription();

            Assert.Equal("New", newDescription);
            Assert.Equal("Shop owner confirmed", confirmedDescription);
            Assert.Equal("Delivered to shipper", receivedDescription);
            Assert.Equal("Customer received", succeedDescription);
            Assert.Equal("Canceled", canceledDescription);
            Assert.Throws<NotImplementedException>(() => InvoiceStatusExtension.GetDescription((InvoiceStatus)10));
        }
    }
}

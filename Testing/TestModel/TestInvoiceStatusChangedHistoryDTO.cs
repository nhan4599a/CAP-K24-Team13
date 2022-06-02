using Shared.DTOs;
using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestInvoiceStatusChangedHistoryDTO
    {
        [Fact]
        public void Test()
        {
            var dateTime = DateTime.Now;
            var invoiceStatusHistoriesDTO = new InvoiceStatusChangedHistoryDTO
            {
                ChangedTime = dateTime,
                Status = InvoiceStatus.Confirmed
            };
            Assert.Equal(dateTime, invoiceStatusHistoriesDTO.ChangedTime);
            Assert.Equal(InvoiceStatus.Confirmed, invoiceStatusHistoriesDTO.Status);
        }
    }
}

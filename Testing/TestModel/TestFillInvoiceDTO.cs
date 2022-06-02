using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestFillInvoiceDTO
    {
        [Fact]
        public void Test()
        {
            var fillInvoiceDTO = new FullInvoiceDTO
            {
                StatusHistories = new List<InvoiceStatusChangedHistoryDTO>()
            };

            Assert.NotNull(fillInvoiceDTO.StatusHistories);
        }
    }
}

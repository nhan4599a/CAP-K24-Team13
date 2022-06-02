using Shared.DTOs;
using Xunit;


namespace TestModel
{
    public class TestInvoiceWithReportDTO
    {
        [Fact]
        public void Test()
        {
            var model = new InvoiceWithReportDTO
            {
                IsReported = true
            };
            Assert.True(model.IsReported);
        }
    }
}

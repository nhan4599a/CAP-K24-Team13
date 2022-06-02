using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestInvoiceWithItemDTO
    {
        [Fact]
        public void Test()
        {
            var model = new InvoiceWithItemDTO
            {
                Products = new List<InvoiceItemDTO>()
            };
            Assert.NotNull(model.Products);
        }
    }
}

using GUI.Models;
using Xunit;

namespace TestModel
{
    public class TestExternalApiPaginatedList
    {
        [Fact]
        public void Test()
        {
            var model = new ExternalApiPaginatedList<string> 
            {
                PageIndex = 123,
                PageSize = 123,
                TotalRecords = 123,
                PageCount = 123
            };
            Assert.Equal(123, model.PageIndex);
            Assert.Equal(123, model.PageSize);
            Assert.Equal(123, model.TotalRecords);
            Assert.Equal(123, model.PageCount);

        }
    }
}

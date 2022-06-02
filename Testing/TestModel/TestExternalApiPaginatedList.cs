using GUI.Models;
using Shared.Models;
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
                PageIndex = 1,
                PageSize = 10,
                TotalRecords = 3,
                PageCount = 1,
                Items = new List<string>
                {
                    "item 1", "item 2", "item 3"
                }
            };
            Assert.Equal(1, model.PageIndex);
            Assert.Equal(10, model.PageSize);
            Assert.Equal(3, model.TotalRecords);
            Assert.Equal(1, model.PageCount);
            Assert.Equal(3, model.Items.Count);

            var internalPaginatedList = model.ToInternal();

            Assert.NotNull(internalPaginatedList);

            Assert.Equal(1, internalPaginatedList.PageNumber);
            Assert.Equal(10, internalPaginatedList.PageSize);
        }
    }
}

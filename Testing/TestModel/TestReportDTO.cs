using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestReportDTO
    {
        [Fact]
        public void Test()
        {
            var model = new ReportDTO
            {
                Id = 123,
                Reporter = "abc",
                AffectedUser = "abc"
            };
            Assert.Equal(123, model.Id);
            Assert.Equal("abc", model.Reporter);
            Assert.Equal("abc", model.AffectedUser);

        }
    }
}

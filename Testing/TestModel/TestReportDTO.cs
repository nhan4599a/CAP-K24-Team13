using Shared.DTOs;
using Xunit;

namespace TestModel
{
    public class TestReportDTO
    {
        [Fact]
        public void Test()
        {
            var dateTime = DateTime.Now;
            var model = new ReportDTO
            {
                Id = 123,
                Reporter = "abc",
                AffectedUser = "abc",
                CreatedAt = dateTime
            };
            Assert.Equal(123, model.Id);
            Assert.Equal("abc", model.Reporter);
            Assert.Equal("abc", model.AffectedUser);
            Assert.Equal(dateTime, model.CreatedAt);
        }
    }
}

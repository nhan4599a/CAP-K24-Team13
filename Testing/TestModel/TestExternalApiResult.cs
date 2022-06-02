using GUI.Models;
using Xunit;

namespace TestModel
{
    public class TestExternalApiResult
    {
        [Fact]
        public void Test()
        {
            var model = new ExternalApiResult<string>
            {
                IsSuccessed = true,
                Message = "abc",
                ResultObj = "abc"
            };
            Assert.True(model.IsSuccessed);
            Assert.Equal("abc", model.Message);
            Assert.Equal("abc", model.ResultObj);
        }
    }
}

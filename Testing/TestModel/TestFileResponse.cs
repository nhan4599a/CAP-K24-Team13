using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestFileResponse
    {
        [Fact]
        public void Test()
        {
            var fileResponse = new FileResponse
            {
                FullPath = "abc.txt",
                MimeType = "text/plain"
            };
            Assert.Equal("abc.txt", fileResponse.FullPath);
            Assert.Equal("text/plain", fileResponse.MimeType);
            Assert.False(fileResponse.IsExisted);
        }
    }
}

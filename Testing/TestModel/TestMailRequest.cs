using Shared.Models;
using Xunit;

namespace TestModel
{
    public class TestMailRequest
    {
        [Fact]
        public void Test()
        {
            var model = new MailRequest
            {
                Receiver = "abc",
                Subject = "abc",
                Body = "abc",
                IsHtmlMessage = true
            };
            Assert.Equal("abc", model.Receiver);
            Assert.Equal("abc", model.Subject);
            Assert.Equal("abc", model.Body);
            Assert.Equal(true, model.IsHtmlMessage);

        }
    }
}

using DatabaseAccessor.Models;
using Xunit;

namespace TestModel
{
    public class TestReport
    {
        [Fact]
        public void Test()
        {
            var affectedUserId = Guid.NewGuid();
            var reporterUserId = Guid.NewGuid();
            var dateTime = DateTime.Now;
            var model = new Report
            {
                Id = 1,
                AffectedInvoice = new Invoice(),
                AffectedInvoiceId = 1,
                AffectedUser = new User(),
                AffectedUserId = affectedUserId,
                CreatedAt = dateTime,
                Reporter = new User(),
                ReporterId = reporterUserId
            };
            Assert.Equal(1, model.Id);
            Assert.NotNull(model.AffectedInvoice);
            Assert.Equal(1, model.AffectedInvoiceId);
            Assert.NotNull(model.AffectedUser);
            Assert.Equal(affectedUserId, model.AffectedUserId);
            Assert.Equal(dateTime, model.CreatedAt);
            Assert.NotNull(model.Reporter);
            Assert.Equal(reporterUserId, model.ReporterId);
        }
    }
}

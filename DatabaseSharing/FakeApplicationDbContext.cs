using DatabaseAccessor;
using DatabaseAccessor.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseSharing
{
    public class FakeApplicationDbContext : ApplicationDbContext
    {
        public FakeApplicationDbContext() : base(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Database").Options)
        { }
    }
}
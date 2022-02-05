using DatabaseAccessor.Contexts;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseAccessor.Factories
{
    internal class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return new ApplicationDbContext("Server=localhost,1433; Database=CAP-K24-Team13; User ID=sa; Password=nhan4599@Nhan; TrustServerCertificate=True");
        }
    }
}

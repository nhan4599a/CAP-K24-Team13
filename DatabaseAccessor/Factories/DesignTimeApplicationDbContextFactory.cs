using DatabaseAccessor.Contexts;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseAccessor.Factories
{
    internal class DesignTimeApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return new ApplicationDbContext();
        }
    }
}

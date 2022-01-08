using DatabaseAccessor.Contexts;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseAccessor.Factories
{
    public class DesignTimeClientAuthenticationDbContextFactory : IDesignTimeDbContextFactory<ClientAuthenticationDbContext>
    {
        public ClientAuthenticationDbContext CreateDbContext(string[] args)
        {
            return new ClientAuthenticationDbContext();
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace DatabaseAccessor.Contexts
{
    public class ClientAuthenticationDbContext : DbContext
    {
        public ClientAuthenticationDbContext(DbContextOptions<ClientAuthenticationDbContext> options) : base(options) { }
    }
}

using Microsoft.EntityFrameworkCore;

namespace DatabaseAccessor.Contexts
{
    public class ClientAuthenticationDbContext : DbContext
    {
        private static readonly string _connectionString = "Server=.,4599;Database=ClientAuthentication;User ID=sa;Password=nhan4599@Nhan;TrustServerCertificate=true";

        public ClientAuthenticationDbContext(string connectionString) : base(
            new DbContextOptionsBuilder<ClientAuthenticationDbContext>().UseSqlServer(connectionString).Options)
        { }

        public ClientAuthenticationDbContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}

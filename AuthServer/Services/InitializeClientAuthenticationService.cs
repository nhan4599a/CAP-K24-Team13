using AuthServer.Configurations;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Services
{
    public class InitializeClientAuthenticationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public InitializeClientAuthenticationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = _serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope()!;
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            dbContext.Database.Migrate();
            if (!dbContext.Clients.Any())
            {
                foreach (var client in ClientAuthConfig.Clients)
                {
                    dbContext.Clients.Add(client.ToEntity());
                }
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            if (!dbContext.IdentityResources.Any())
            {
                foreach (var identityResource in ClientAuthConfig.IdentityResources)
                {
                    dbContext.IdentityResources.Add(identityResource.ToEntity());
                }
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            if (!dbContext.ApiScopes.Any())
            {
                foreach (var apiScope in ClientAuthConfig.ApiScopes)
                {
                    dbContext.ApiScopes.Add(apiScope.ToEntity());
                }
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            if (!dbContext.ApiResources.Any())
            {
                foreach (var apiResource in ClientAuthConfig.ApiResources)
                {
                    dbContext.ApiResources.Add(apiResource.ToEntity());
                }
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

using AuthServer.Identities;
using DatabaseAccessor.Models;

namespace AuthServer.Services
{
    public class InitializeAccountChallengeService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public InitializeAccountChallengeService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<ApplicationUserManager>();
            var roleManager = scope.ServiceProvider.GetRequiredService<ApplicationRoleManager>();
            await InitializeRoles(roleManager);
            await InitializeTestUser(userManager);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task InitializeTestUser(ApplicationUserManager userManager)
        {
            string password = "CapK24Team13@Default";
            if (await userManager.FindByNameAsync("customer_test") == null)
            {
                var user = await CreateUserObj("customer_test");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Customer");
            }
            if (await userManager.FindByNameAsync("admin_test") == null)
            {
                var user = await CreateUserObj("admin_test");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "Admin");
            }
            if (await userManager.FindByNameAsync("owner_test") == null)
            {
                var user = await CreateUserObj("owner_test");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "ShopOwner");
            }
        }

        private static async Task InitializeRoles(ApplicationRoleManager roleManager)
        {
            if (await roleManager.FindByNameAsync("Customer") == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                });
            }
            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
            }
            if (await roleManager.FindByNameAsync("ShopOwner") == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "ShopOwner",
                    NormalizedName = "SHOP_OWNER"
                });
            }
        }

        private static Task<User> CreateUserObj(string username)
        {
            var email = $"{username.Replace('_', '@')}.com";
            return Task.FromResult(new User
            {
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                Email = email,
                NormalizedEmail = email.ToUpper(),
                FirstName = "Test",
                LastName = "Test"
            });
        }
    }
}

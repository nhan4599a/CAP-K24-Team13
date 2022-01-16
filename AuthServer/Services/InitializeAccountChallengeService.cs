using AuthServer.Identities;
using DatabaseAccessor.Models;

namespace AuthServer.Services
{
    public class InitializeAccountChallengeService : IHostedService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;

        public InitializeAccountChallengeService(
            ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeRoles();
            await InitializeTestUser();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task InitializeTestUser()
        {
            string password = "CapK24Team13@Default";
            if (_userManager.FindByNameAsync("customer_test") == null)
            {
                var user = await CreateUserObj("customer_test");
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "Customer");
            }
            if (_userManager.FindByNameAsync("admin_test") == null)
            {
                var user = await CreateUserObj("admin_test");
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            if (_userManager.FindByNameAsync("owner_test") == null)
            {
                var user = await CreateUserObj("owner_test");
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "ShopOwner");
            }
        }

        private async Task InitializeRoles()
        {
            if (_roleManager.FindByNameAsync("Customer") == null)
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                });
            }
            if (_roleManager.FindByNameAsync("Admin") == null)
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
            }
            if (_roleManager.FindByNameAsync("ShopOwner") == null)
            {
                await _roleManager.CreateAsync(new Role
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
                NormalizedEmail = email.ToUpper()
            });
        }
    }
}

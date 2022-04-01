using AuthServer.Identities;
using DatabaseAccessor.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            await InitializeTestUsers(userManager);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task InitializeTestUsers(ApplicationUserManager userManager)
        {
            string password = "CapK24Team13@Default";
            if (await userManager.FindByNameAsync("customer_test") == null)
            {
                var user = await CreateUserObj("customer_test");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, SystemConstant.Roles.CUSTOMER);
            }
            if (await userManager.FindByNameAsync("owner_test") == null)
            {
                var user = await CreateUserObj("owner_test");
                user.ShopId = 1;
                user.Id = Guid.Parse("751e9157-b88e-4e5c-46d3-08da12252e89");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, SystemConstant.Roles.SHOP_OWNER);
            }
            if (await userManager.FindByNameAsync("admin_team13") == null)
            {
                var user = await CreateUserObj("admin_team13");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, SystemConstant.Roles.ADMIN_TEAM_13);
            }
            if (await userManager.FindByNameAsync("admin_team5") == null)
            {
                var user = await CreateUserObj("admin_team5");
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, SystemConstant.Roles.ADMIN_TEAM_5);
            }
        }

        private static async Task InitializeRoles(ApplicationRoleManager roleManager)
        {
            if (await roleManager.FindByNameAsync(SystemConstant.Roles.CUSTOMER) == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = SystemConstant.Roles.CUSTOMER,
                    NormalizedName = SystemConstant.Roles.CUSTOMER.ToUpper()
                });
            }
            if (await roleManager.FindByNameAsync(SystemConstant.Roles.SHOP_OWNER) == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = SystemConstant.Roles.SHOP_OWNER,
                    NormalizedName = SystemConstant.Roles.SHOP_OWNER.ToUpper()
                });
            }
            if (await roleManager.FindByNameAsync(SystemConstant.Roles.ADMIN_TEAM_13) == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = SystemConstant.Roles.ADMIN_TEAM_13,
                    NormalizedName = SystemConstant.Roles.ADMIN_TEAM_13.ToUpper()
                });
            }
            if (await roleManager.FindByNameAsync(SystemConstant.Roles.ADMIN_TEAM_5) == null)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = SystemConstant.Roles.ADMIN_TEAM_5,
                    NormalizedName = SystemConstant.Roles.ADMIN_TEAM_5.ToUpper()
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
                LastName = "Test",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                DoB = DateOnly.MinValue
            });
        }
    }
}

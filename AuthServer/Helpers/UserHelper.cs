using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer.Helpers
{
    public static class UserHelper
    {
        public static async Task<CreateUserResult> CreateUserAsync(this UserManager<User> userManager, UserSignUpModel model, string role)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var user = new User
            {
                Email = model.Email,
                UserName = model.Username,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Username.ToUpper(),
                DoB = model.DoB,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var createAccountResult = await userManager.CreateAsync(user, model.Password);
            if (createAccountResult.Succeeded)
            {
                await userManager.AddClaimsAsync(user, new[]
                {
                    new Claim(ClaimTypes.Email, model.Email)
                });
                var addToRoleResult = await userManager.AddToRoleAsync(user, role);
                if (addToRoleResult.Succeeded)
                    return CreateUserResult.Success(user);
                return CreateUserResult.Failed(addToRoleResult.Errors);
            }
            return CreateUserResult.Failed(createAccountResult.Errors);
        }

        public static async Task<CreateUserResult> CreateUserAsync(this UserManager<User> userManager, ShopOwnerSignUpModel model,
            string username, string password)
        {
            return await userManager.CreateUserAsync(new UserSignUpModel
            (
                model.FirstName,
                model.LastName,
                username,
                password,
                model.Email,
                model.PhoneNumber,
                model.DoB
            ), Roles.SHOP_OWNER);
        }
    }
}

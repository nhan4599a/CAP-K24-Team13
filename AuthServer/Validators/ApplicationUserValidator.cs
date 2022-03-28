using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AuthServer.Validators
{
    public class ApplicationUserValidator : IUserValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            var foundUser = await manager.FindByEmailAsync(user.Email);
            if (foundUser != null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "EmailAlreadyExisted",
                    Description = "Email is already used by another account"
                });
            return IdentityResult.Success;
        }
    }
}

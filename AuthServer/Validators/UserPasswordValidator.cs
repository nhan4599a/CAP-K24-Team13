using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Validators
{
    public class UserPasswordValidator : IPasswordValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            var usernane = await manager.GetUserNameAsync(user);
            if (password.ToLower().Contains(usernane.ToLower()))
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsUsername",
                    Description = "Password can't be contains username"
                });
            if (password.ToLower().Contains("password"))
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsPassword",
                    Description = "Password can't be contains \"password\""
                });
            return IdentityResult.Success;
        }
    }
}

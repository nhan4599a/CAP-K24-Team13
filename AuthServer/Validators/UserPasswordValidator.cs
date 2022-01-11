using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Validators
{
    public class UserPasswordValidator : IPasswordValidator<User>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            var usernane = await manager.GetUserNameAsync(user);
            if (usernane.ToLower().Equals(password.ToLower()))
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "SameUsernameAndPassword",
                    Description = "Username and password can't be the same"
                });
            if (password.ToLower().Contains("password"))
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsPassword",
                    Description = "Password can't be contains \"password\""
                });
            if (password.ToLower().Contains(user.UserName.ToLower()))
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordContainsUsername",
                    Description = "Password can't be contains username"
                });
            return IdentityResult.Success;
        }
    }
}

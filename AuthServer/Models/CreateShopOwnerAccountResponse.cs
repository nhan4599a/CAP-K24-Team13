using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace AuthServer.Models
{
    public class CreateShopOwnerAccountResponse
    {
        public IEnumerable<IdentityError> Errors { get; set; } = Enumerable.Empty<IdentityError>();

        public string? UserName { get; set; }

        public bool IsSucceed => !Errors.Any();

        public static CreateShopOwnerAccountResponse CreateSuccessResult(string userName)
        {
            return new CreateShopOwnerAccountResponse
            {
                UserName = userName
            };
        }

        public static CreateShopOwnerAccountResponse CreateFailedResponse(IEnumerable<IdentityError> errors)
        {
            return new CreateShopOwnerAccountResponse
            {
                Errors = errors
            };
        }
    }
}

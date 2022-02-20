using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace AuthServer.Models
{
    public class CreateUserResult
    {
        public User? User { get; init; }

        public IReadOnlyList<IdentityError> Errors { get; init; } = new List<IdentityError>();

        public bool Succeeded { get; init; }

        protected CreateUserResult() { }

        public static CreateUserResult Success(User user)
        {
            return new CreateUserResult
            {
                User = user,
                Succeeded = true
            };
        }

        public static CreateUserResult Failed(params IdentityError[] errors)
        {
            return new CreateUserResult
            {
                Errors = errors
            };
        }

        public static CreateUserResult Failed(IEnumerable<IdentityError> errors) => Failed(errors.ToArray());
    }
}

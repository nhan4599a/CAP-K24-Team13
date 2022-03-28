using System;

namespace AuthServer.Models
{
    public record ShopOwnerSignUpModel(int ShopId, string FirstName, string LastName, string PhoneNumber, string Email, DateOnly DoB)
    {
    }
}

using System;

namespace AuthServer.Models
{
    public record ShopOwnerSignUpModel(string FirstName, string LastName, string PhoneNumber, string Location, string Email, DateOnly DoB)
    {
    }
}

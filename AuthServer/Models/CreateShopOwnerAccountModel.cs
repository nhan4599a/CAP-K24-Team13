using System;

namespace AuthServer.Models
{
    public record CreateShopOwnerAccountModel(string PhoneNumber, string Location, string Email, string Name, DateOnly DoB)
    {
    }
}

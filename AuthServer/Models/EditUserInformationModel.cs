using System;

namespace AuthServer.Models
{
    public record EditUserInformationModel(string FirstName, string LastName, DateOnly DoB, string PhoneNumber)
    {
    }
}
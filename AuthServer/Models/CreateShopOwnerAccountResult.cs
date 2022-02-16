using System;

namespace AuthServer.Models
{
    public record CreateShopOwnerAccountResult(Guid UserId, string Username)
    {
    }
}
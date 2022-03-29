﻿using Refit;
using Shared.Models;
using System.Threading.Tasks;

namespace ReportService.Clients
{
    public interface IUserClient
    {
        [Post("/api/users/ban/{userId}")]
        Task ApplyBan([Authorize("Bearer")] string token, string userId, [Body] AccountPunishmentBehavior behavior);
    }
}

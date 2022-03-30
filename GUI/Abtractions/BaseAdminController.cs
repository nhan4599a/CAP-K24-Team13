using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GUI.Abtractions
{
    [Authorize(Roles = Roles.ADMIN_TEAM_13)]
    [Area("Admin")]
    public class BaseAdminController : Controller
    {
    }
}

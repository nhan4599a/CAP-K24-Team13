using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GUI.Abtractions
{
    [Authorize(Roles = Roles.ADMIN_TEAM_13)]
    [Area(Roles.ADMIN_TEAM_13)]
    public class BaseAdminController : Controller
    {
    }
}

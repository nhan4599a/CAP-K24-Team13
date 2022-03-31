using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace GUI.Abtractions
{
    [Authorize(Roles = SystemConstant.Roles.ADMIN_TEAM_13)]
    [Area("Admin")]
    public class BaseAdminController : Controller
    {
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GUI.Abtractions
{
    [Authorize(Roles = Roles.ADMIN)]
    [Area(Roles.ADMIN)]
    public class BaseAdminController : Controller
    {
    }
}

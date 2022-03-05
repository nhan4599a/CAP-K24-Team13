using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GUI.Abtractions
{
    [Authorize(Roles = Roles.SHOP_OWNER)]
    [Area(Roles.SHOP_OWNER)]
    public class BaseShopOwnerController : Controller
    {
    }
}

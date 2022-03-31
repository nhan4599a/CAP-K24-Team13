using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace GUI.Abtractions
{
    [Authorize(Roles = SystemConstant.Roles.SHOP_OWNER)]
    [Area(SystemConstant.Roles.SHOP_OWNER)]
    public class BaseShopOwnerController : Controller
    {
    }
}

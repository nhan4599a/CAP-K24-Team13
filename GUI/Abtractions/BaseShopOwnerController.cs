using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Abtractions
{
    [Authorize(Roles = "ShopOwner")]
    [Area("ShopOwner")]
    public class BaseShopOwnerController : Controller
    {
    }
}

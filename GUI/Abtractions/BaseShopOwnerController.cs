using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Abtractions
{
    [Authorize(Roles = "ShopOwner")]
    [Area("Admin")]
    public class BaseShopOwnerController : Controller
    {
    }
}

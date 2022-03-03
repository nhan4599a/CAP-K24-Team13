using GUI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Abtractions
{
    [VirtualArea("User")]
    [ServiceFilter(typeof(BaseUserActionFilter))]
    public class BaseUserController : Controller
    {
    }
}

using GUI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Abtractions
{
    [VirtualArea("User")]
    [ServiceFilter(typeof(BaseActionFilter))]
    public class BaseUserController : Controller
    {
    }
}

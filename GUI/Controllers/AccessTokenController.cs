using GUI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace GUI.Controllers
{
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        [Route("/token")]
        public string GetCurrentUserAccessToken()
        {
            if (User.Identity.IsAuthenticated)
                return string.Empty;
            return User.GetUserId().ToString();
        }
    }
}

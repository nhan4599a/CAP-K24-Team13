using GUI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace GUI.Controllers
{
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        [Route("/token")]
        public ApiResult GetCurrentUserAccessToken()
        {
            if (!User.Identity.IsAuthenticated)
                return ApiResult.CreateErrorResult(403, "User is not logged in");
            return ApiResult<string>.CreateSucceedResult(User.GetUserId().ToString());
        }
    }
}

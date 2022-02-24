using GUI.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Threading.Tasks;

namespace GUI.Controllers
{
    [ApiController]
    public class AccessTokenController : ControllerBase
    {
        [Route("/token")]
        public async Task<ApiResult> GetCurrentUserAccessToken()
        {
            if (!User.Identity.IsAuthenticated)
                return ApiResult.CreateErrorResult(403, "User is not logged in");
            return ApiResult<string>.CreateSucceedResult(await HttpContext.GetTokenAsync("access_token"));
        }
    }
}

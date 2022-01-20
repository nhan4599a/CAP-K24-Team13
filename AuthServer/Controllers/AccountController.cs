using AuthServer.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Information()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ActionName("Information")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInformation()
        {
            return View();
        }
    }
}

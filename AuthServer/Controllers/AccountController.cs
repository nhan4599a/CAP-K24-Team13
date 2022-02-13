using AuthServer.Identities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Information()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Information")]
        [ValidateAntiForgeryToken]
        public IActionResult EditInformation()
        {
            return View();
        }

        [ActionName("Password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}

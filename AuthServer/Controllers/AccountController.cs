using AuthServer.Identities;
using AuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpGet]
        [ActionName("change-password")]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword");
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("ChangePassword-Error", "Something went wrong");
                return View("ChangePassword");
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, model.Password, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError("ChangePassword-Error", error.Description);
            }
            return View("ChangePassword");
        }
    }
}

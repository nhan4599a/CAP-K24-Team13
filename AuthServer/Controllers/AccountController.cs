using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager; 

        public AccountController(ApplicationUserManager userManager,ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("ChangePassword-Error", "Something went wrong");
                return View();
            }
            var changePasswordResult = await _userManager.ChangePasswordAsync(currentUser, model.Password, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError("ChangePassword-Error", error.Description);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    Password = model.Password,
                    RePassword = model.RePassword,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Register failed, try again!");

            }
            return View(model);
        }

    }
}

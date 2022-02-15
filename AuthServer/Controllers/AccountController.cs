using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Microsoft.AspNetCore.Identity;
using Azure;
using Consul;
using IdentityServer4.Models;

namespace AuthServer.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationRoleManager _roleManager;
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

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
        public async Task<IActionResult> Register([FromBody] SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    RePassword = model.RePassword,
                    DoB = model.DoB,
                };


                var result = await _userManager.CreateAsync(user, model.Password);
                //var roles = await _userManager.GetRolesAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "CUSTOMER");

                    return Ok();
                    //    foreach (var error in result.Errors)
                    //{
                    //        ModelState.AddModelError(string.Empty, error.Description);
                    //}
                    //        ModelState.AddModelError(string.Empty, "Register failed, try again!");
                }
                else
                {
                    return BadRequest();
                }
            }
            return Ok();
        }
    }
}




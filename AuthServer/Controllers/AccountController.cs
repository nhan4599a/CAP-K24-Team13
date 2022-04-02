using AuthServer.Abstractions;
using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Contexts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    [AuthorizeWithoutRedirectToSignIn]
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _dbContext;
        
        public AccountController(ApplicationUserManager userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Information()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            return View(currentUser);
        }

        [HttpPost]
        [ActionName("Information")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInformation(EditUserInformationModel model, [FromQuery] string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("ChangeInformation-Error", "Something went wrong");
                return View();
            }
            _dbContext.Attach(currentUser);
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.DoB = model.DoB;
            currentUser.PhoneNumber = model.PhoneNumber;
            await _dbContext.SaveChangesAsync();
            return Redirect(returnUrl);
        }

        [ActionName("change-password")]
        public IActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model, [FromQuery] string returnUrl)
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
                return View("ChangePassword");
            }
            return Redirect(returnUrl);
        }
    }
}

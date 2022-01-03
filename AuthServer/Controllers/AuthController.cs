using DatabaseAccessor.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Identity = Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Events;
using IdentityServer4;
using AuthServer.Models;

namespace AuthServer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;
        private readonly Identity.SignInManager<User> _signInManager;

        public AuthController(IIdentityServerInteractionService interaction, IEventService events,
            Identity.SignInManager<User> signInManager)
        {
            _interaction = interaction;
            _events = events;
            _signInManager = signInManager;
        }

        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignIn-Error", "Username or password is invalid");
                return SignIn(model.ReturnUrl);
            }
            var authContext = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (user != null && signInResult.Succeeded)
            {
                await _events.RaiseAsync(
                    new UserLoginSuccessEvent(model.Username, user.Id.ToString(), model.Username,
                        clientId: authContext?.Client.ClientId));
                var props = model.RememberMe ? new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1))
                } : null;
                var issuer = new IdentityServerUser(user.Id.ToString())
                {
                    DisplayName = user.UserName
                };
                await HttpContext.SignInAsync(issuer, props);
                return Redirect(string.IsNullOrEmpty(model.ReturnUrl) ? "~/" : model.ReturnUrl);
            }
            return SignIn(model.ReturnUrl);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("SignUp-Error", "Username or password is invalid");
                return SignUp();
            }
            var user = new User
            {
                Email = model.Email,
                UserName = model.Username,
                NormalizedEmail = model.Email,
                NormalizedUserName = model.Username
            };
            var identityResult = await _signInManager.UserManager.CreateAsync(user, model.Password);
            if (identityResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }
            foreach (var error in identityResult.Errors)
                ModelState.AddModelError("SignUp-Error", error.Description);
            return SignUp();
        }

        private async Task SendUserValidationEmail(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException($"{nameof(user.Email)} cannot be null or empty");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

        }
    }
}

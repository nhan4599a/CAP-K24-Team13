using AuthServer.Models;
using DatabaseAccessor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Identity = Microsoft.AspNetCore.Identity;

namespace AuthServer.Controllers
{
    [Route("/auth")]
    public class AuthenticationController : Controller
    {
        private readonly Identity.SignInManager<User> _signInManager;
        private readonly IMailService _mailer;

        public AuthenticationController(Identity.SignInManager<User> signInManager, IMailService mailer)
        {
            _signInManager = signInManager;
            _mailer = mailer;
        }

        [AllowAnonymous]
        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignIn-Error", "Username or password is invalid");
                return View(model);
            }
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (user != null && signInResult.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, (await _signInManager.UserManager.GetRolesAsync(user))[0])
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                throw new InvalidOperationException($"\"{model.ReturnUrl}\" is not valid");
            }
            ModelState.AddModelError("SignIn-Error", "Username and/or password is incorrect");
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                ModelState.AddModelError("SignUp-Error", "Username or password is invalid");
                return View(model);
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
                await SendUserConfirmationEmail(user);
                return RedirectToAction("SignIn");
            }
            foreach (var error in identityResult.Errors)
                ModelState.AddModelError("SignUp-Error", error.Description);
            return View(model);
        }

        private async Task SendUserConfirmationEmail(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException($"{nameof(user.Email)} cannot be null or empty");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var message = await GenerateEmailAsync(user.Email, token);
            _mailer.SendMail(message);
        }

        private Task<MailRequest> GenerateEmailAsync(string receiver, string token)
        {
            var body = "Thanks for your registration," +
                " this is your email confirmation link" +
                $" <a href=\"{$"https://localhost:7265/auth/confirmation?token={token}"}\"></a>." +
                $" The link will be expired at {DateTime.UtcNow.AddMinutes(30):dddd, MMMM d, yyyy; HH:mm:ss tt}";
            return Task.FromResult<MailRequest>(new()
            {
                Body = body,
                Sender = _mailer.MailAddress,
                IsHtmlMessage = true,
                Receiver = receiver,
                Subject = "Email confirmation"
            });
        }
    }
}

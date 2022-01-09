using AuthServer.Configurations;
using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IMailService _mailer;
        private readonly IEventService _events;

        public AuthenticationController(IIdentityServerInteractionService interaction,
            ApplicationSignInManager signInManager, IEventService eventService,IMailService mailer)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _events = eventService;
            _mailer = mailer;
        }

        [AllowAnonymous]
        [Route("/auth/signin")]
        public IActionResult SignIn(string returnUrl = "~/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/signin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignIn-Error", "Username or password is invalid");
                return View(model);
            }
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (context == null)
            {
                ModelState.AddModelError("SignIn-Error", "Something went wrong!");
                return View(model);
            }
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password,
                AccountConfig.AccountLockedOutEnabled);
            if (user != null && signInResult.Succeeded)
            {
                await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(),
                    user.UserName, clientId: context.Client.ClientId));
                AuthenticationProperties? props = null;
                if (AccountConfig.AllowRememberMe && model.RememberMe)
                    props = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountConfig.RememberMeDuration)
                    };
                var identityServerUser = new IdentityServerUser(user.Id.ToString())
                {
                    DisplayName = user.UserName
                };
                await HttpContext.SignInAsync(identityServerUser, props);
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                throw new InvalidOperationException($"\"{model.ReturnUrl}\" is not valid");
            }
            else if (user != null && signInResult.IsLockedOut)
            {
                ModelState.AddModelError("SignIn-Error", "Account is locked out");
                ViewBag.LockedOutCancelTime = user.LockoutEnd!.Value;
                return View(model);
            }
            ModelState.AddModelError("SignIn-Error", "Username and/or password is incorrect");
            return View(model);
        }

        [AllowAnonymous]
        [Route("/auth/signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/signup")]
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
                NormalizedUserName = model.Username,
                DoB = model.DoB
            };
            var identityResult = await _signInManager.UserManager.CreateAsync(user, model.Password);
            if (identityResult.Succeeded)
            {
                if (AccountConfig.RequireEmailConfirmation)
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

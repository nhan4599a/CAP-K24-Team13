using AuthServer.Abstractions;
using AuthServer.Configurations;
using AuthServer.Helpers;
using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    [ServiceFilter(typeof(SignInActionFilter))]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IMailService _mailer;
        private readonly IEventService _events;

        public AuthenticationController(IIdentityServerInteractionService interaction,
            ApplicationSignInManager signInManager, IEventService eventService, IMailService mailer)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _events = eventService;
            _mailer = mailer;
        }

        [Route("/auth/SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [Route("/auth/SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignIn-Error", "Username or password is invalid");
                return View();
            }
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("SignIn-Error", "Username or password is incorrect");
                return View();
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, AccountConfig.AccountLockedOutEnabled);
            if (signInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                throw new InvalidOperationException($"\"{model.ReturnUrl}\" is  invalid");
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("SignIn-Error", "Account is locked out");
                ViewBag.LockedOutCancelTime = user!.LockoutEnd!.Value;
                return View(model);
            }
            if (signInResult.IsNotAllowed)
            {
                ModelState.AddModelError("SignIn-Error", "Account is have not been confirmed");
                return View(model);
            }
            ModelState.AddModelError("SignIn-Error", "Username and/or password is incorrect");
            return View();
        }

        [Route("/auth/SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("/auth/SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignUp-Error", "Input information is invalid");
                return View(model);
            }
            var createUserResult = await _signInManager.UserManager.CreateUserAsync(model, Roles.CUSTOMER);
            if (createUserResult.Succeeded)
            {
                if (AccountConfig.RequireEmailConfirmation)
                    await SendUserConfirmationEmail(createUserResult.User!);
                return RedirectToAction("SignIn");
            }
            foreach (var error in createUserResult.Errors)
                ModelState.AddModelError("SignUp-Error", error.Description);
            return View(model);
        }

        [HttpGet]
        [Route("/auth/signout")]
        public async Task<IActionResult> SignOut(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

            if (logoutContext == null)
                throw new InvalidOperationException("Something went wrong!");

            return Redirect(logoutContext.PostLogoutRedirectUri);
        }

        
        [HttpGet("/Auth/Confirmation/{email}")]
        public async Task<IActionResult> ConfirmEmail(string email, [FromQuery] string token)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError("ConfirmEmail-Error", $"Something went wrong");
                return View();
            }
            var user = await _signInManager.UserManager.FindByEmailAsync(Base64Decode(email));
            if (user == null)
            {
                ModelState.AddModelError("ConfirmEmail-Error",$"User not found");
                return View();
            }
            var result = await _signInManager.UserManager.ConfirmEmailAsync(user, Base64Decode(token));
            if(result.Succeeded)
            {
                return View();
            }
            foreach(var error in result.Errors)
                ModelState.AddModelError("ConfirmEmail-Error", error.Description);
            return View();
        }

        private async Task SendUserConfirmationEmail(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException($"{nameof(user.Email)} cannot be null or empty");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var message = GenerateEmailAsync(user.Email, Base64Encode(token));
            _mailer.SendMail(message);
        }

        private static MailRequest GenerateEmailAsync(string receiver, string token)
        {
            var email = Base64Encode(receiver);
            var body = "Thanks for your registration," +
                $" this is your email confirmation <a href=\"{$"https://localhost:7265/auth/confirmation/{email}?token={token}"}\">link</a>" +
                $" The link will be expired at {DateTime.UtcNow.AddMinutes(30):dddd, MMMM d, yyyy; HH:mm:ss tt}";
            return new MailRequest()
            {
                Body = body,
                Sender = "gigamallservice@gmail.com",
                IsHtmlMessage = true,
                Receiver = receiver,
                Subject = "Email confirmation"
            };
        }

        private static string Base64Encode(string original)
        {
            return Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(original));
        }

        private static string Base64Decode(string encodedString)
        {
            return System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(encodedString));
        }
    }
}

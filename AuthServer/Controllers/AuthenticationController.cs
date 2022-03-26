using AspNetCoreSharedComponent.HttpContext;
using AuthServer.Configurations;
using AuthServer.Extensions;
using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Models;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IMailService _mailer;
        private readonly IEventService _event;

        private const string SignInParamsKey = "SIGN_IN_PARAMS_KEY";

        public AuthenticationController(IIdentityServerInteractionService interaction,
            ApplicationSignInManager signInManager, IMailService mailer, IEventService @event)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _mailer = mailer;
            _event = @event;
        }

        [Route("/Auth/SignIn")]
        public IActionResult SignIn()
        {
            var paramString = Request.QueryString.ToString();
            HttpContext.Session.SetString(SignInParamsKey, paramString);
            return View();
        }

        [HttpPost]
        [Route("/Auth/SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignIn-Error", "Username and/or password is invalid");
                return View();
            }
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (context == null)
            {
                ModelState.AddModelError("SignIn-Error", "Something went wrong!");
                return View();
            }
            if (user == null)
            {
                ModelState.AddModelError("SignIn-Error", "Username and/or password is incorrect");
                return View();
            }
            if (user.Status == AccountStatus.Banned && user.LockoutEnd == null)
            {
                ModelState.AddModelError("SignIn-Error", 
                    "Look like your account is locked out permanently. Contact admin for more detail");
                return View();
            }
            var signInResult =  await 
                _signInManager.PasswordSignInAsync(user, model.Password, true, AccountConfig.AccountLockedOutEnabled);
            if (signInResult.Succeeded)
            {
                await _event.RaiseAsync(
                    new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName, clientId: context.Client.ClientId)
                );
                return Redirect(model.ReturnUrl);
            }
            if (signInResult.IsLockedOut || user.Status == AccountStatus.Banned)
            {
                ModelState.AddModelError("SignIn-Error", 
                    $"Account is locked out. It will be unlocked at {user.LockoutEnd!.Value.AddHours(7):dd/MM/yyyy HH:mm:ss}");
                return View();
            }
            if (signInResult.IsNotAllowed)
            {
                ModelState.AddModelError("SignIn-Error", "Account is have not been confirmed");
                return View();
            }
            ModelState.AddModelError("SignIn-Error", "Username and/or password is incorrect");
            return View();
        }

        [Route("/Auth/SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [Route("/Auth/SignUp")]
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
                return Redirect($"/auth/signin{HttpContext.Session.GetAndRemove<string>(SignInParamsKey)}");
            }
            foreach (var error in createUserResult.Errors)
                ModelState.AddModelError("SignUp-Error", error.Description);
            return View(model);
        }

        [Route("/Auth/SignOut")]
        public async Task<IActionResult> SignOut(string logoutId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var userName = User.Identity!.Name;
            await _signInManager.SignOutAsync();

            var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

            if (logoutContext == null)
                throw new InvalidOperationException("Something went wrong!");

            await _event.RaiseAsync(new UserLogoutSuccessEvent(userId, userName));

            if (string.IsNullOrWhiteSpace(logoutContext.PostLogoutRedirectUri))
            {
                return View("SignedOut");
            }

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
            var user = await _signInManager.UserManager.FindByEmailAsync(StringExtension.FromBase64(email));
            if (user == null)
            {
                ModelState.AddModelError("ConfirmEmail-Error",$"User not found");
                return View();
            }
            var result = await _signInManager.UserManager.ConfirmEmailAsync(user, StringExtension.FromBase64(token));
            if(result.Succeeded)
            {
                return View();
            }
            foreach(var error in result.Errors)
                ModelState.AddModelError("ConfirmEmail-Error", error.Description);
            return View();
        }

        [HttpGet]
        [Route("/Auth/Forgot")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("/Auth/Forgot")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("ForgotPassword-Error", "Email does not linked to any account");
                return View();
            }
            await SendResetPasswordEmail(user);
            return View("ForgotPasswordNotification");
        }

        [Route("/Auth/Reset/{email}")]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Auth/Reset/{email}")]
        public async Task<IActionResult> ResetPassword([FromForm(Name = "password")] string newPassword,
            [FromForm(Name = "re-password")] string reNewPassword, string email,
            [FromQuery] string token)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(StringExtension.FromBase64(email));
            if (user == null)
            {
                ModelState.AddModelError("ResetPassword-Error", "Something went wrong!");
                return View();
            }
            if (newPassword != reNewPassword)
            {
                ModelState.AddModelError("ResetPassword-Error", "Password does not match!");
                return View();
            }
            var resetPasswordResult = 
                await _signInManager.UserManager.ResetPasswordAsync(user, StringExtension.FromBase64(token), newPassword);
            if (resetPasswordResult.Succeeded)
            {
                return Redirect($"/auth/signin{HttpContext.Session.GetAndRemove<string>(SignInParamsKey)}");
            }
            foreach (var error in resetPasswordResult.Errors)
                ModelState.AddModelError("ResetPassword-Error", error.Description);
            return View();
        }

        private async Task SendUserConfirmationEmail(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException($"{nameof(user.Email)} cannot be null or empty");
            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
            var message = GenerateEmailConfirmationMailAsync(user.Email, token);
            _mailer.SendMail(message);
        }

        private static MailRequest GenerateEmailConfirmationMailAsync(string receiver, string token)
        {
            var email = receiver.ToBase64();
            var body = "Thanks for your registration," +
                $" this is your email confirmation <a href=\"{$"https://cap-k24-team13-auth.herokuapp.com/auth/confirmation/{email}?token={token.ToBase64()}"}\">link</a>" +
                $" The link will be expired at {DateTime.UtcNow.AddMinutes(30):dddd, MMMM d, yyyy; HH:mm:ss}";
            return new MailRequest()
            {
                Body = body,
                Sender = "gigamallservice@gmail.com",
                IsHtmlMessage = true,
                Receiver = receiver,
                Subject = "Email confirmation"
            };
        }

        private async Task SendResetPasswordEmail(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ArgumentException($"{nameof(user.Email)} cannot be null or empty");
            var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
            var message = GenerateResetPasswordMailAsync(user.Email, token);
            _mailer.SendMail(message);
        }

        private static MailRequest GenerateResetPasswordMailAsync(string receiver, string token)
        {
            var email = receiver.ToBase64();
            var body = "You are receiving this email because we received a password reset request for your account." +
                 $" This is your link to reset your password <a href=\"{$"https://cap-k24-team13-auth.herokuapp.com/auth/reset/{email}?token={token.ToBase64()}"}\">link</a>. " +
                 $" This link will be expired at {DateTime.UtcNow.AddMinutes(10):dddd, MMMM d, yyyy; HH:mm:ss}" +
                 $" If you did not request a password reset, no further action is required. Regards!";
            return new MailRequest()
            {
                Body = body,
                Sender = "gigamallservice@gmail.com",
                IsHtmlMessage = true,
                Receiver = receiver,
                Subject = "Reset password"
            };
        }
    }
}

using AuthServer.Abstractions;
using AuthServer.Configurations;
using AuthServer.Helpers;
using AuthServer.Identities;
using AuthServer.Models;
using AuthServer.ViewModels;
using DatabaseAccessor.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, AccountConfig.AccountLockedOutEnabled);
            if (user != null && signInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                throw new InvalidOperationException($"\"{model.ReturnUrl}\" is  invalid");
            }
            if (user != null && signInResult.IsLockedOut)
            {
                ModelState.AddModelError("SignIn-Error", "Account is locked out");
                ViewBag.LockedOutCancelTime = user!.LockoutEnd!.Value;
                return View(model);
            }
            if (user != null && signInResult.IsNotAllowed)
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

        [HttpPost]
        [Route("/auth/ExternalSignIn")]
        public IActionResult ExternalSignIn(string provider, string returnUrl = "~/")
        {
            if (Request.QueryString.Value != null)
                returnUrl = Request.QueryString.Value![11..];
            if (!Url.IsLocalUrl(returnUrl) && !_interaction.IsValidReturnUrl(returnUrl))
            {
                throw new InvalidOperationException($"\"{returnUrl}\" is invalid!");
            }
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(ExternalSignInCallback)),
                Items =
                {
                    { "returnUrl", returnUrl },
                    { "provider", provider }
                }
            };
            return Challenge(props, provider);
        }

        public async Task<IActionResult> ExternalSignInCallback()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
            if (!result!.Succeeded)
            {
                throw new Exception("External sign-in failed!");
            }
            var providerName = result.Properties.Items["provider"];
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";
            if (providerName == null)
            {
                throw new Exception("Can't not find SignIn provider!");
            }
            var providerUserClaim = result.Principal.FindFirst(JwtClaimTypes.Subject) ??
                result.Principal.FindFirst(ClaimTypes.NameIdentifier) ??
                throw new Exception("External user id not found!");
            var providerUserId = providerUserClaim.Value;
            var identityResult = await _signInManager.ExternalLoginSignInAsync(providerName, providerUserId, false, true);
            if (identityResult.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                var email = result.Principal.HasClaim(claim => claim.Type == ClaimTypes.Email) ?
                    result.Principal.FindFirst(ClaimTypes.Email)!.Value : null;
                var sessionIdClaim = result.Principal.Claims.FirstOrDefault(claim => claim.Type == JwtClaimTypes.SessionId);
                var idToken = result.Properties.GetTokenValue("id_token");
                var model = new ExternalSignInCreateAccountViewModel(providerName, providerUserId)
                {
                    Email = email,
                    ReturnUrl = returnUrl,
                    SessionId = sessionIdClaim != null ? sessionIdClaim.Value : "",
                    IdToken = idToken ?? ""
                };
                return View("ExternalConfirmation", model);
            }
        }

        [HttpPost]
        [Route("/auth/ExternalConfirmation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalConfirmation(ExternalSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignUp-Error", "Input information is invalid");
                return View();
            }
            var createUserResult = await _signInManager.UserManager.CreateUserAsync(model, Roles.CUSTOMER);
            if (createUserResult.Succeeded)
            {
                var user = createUserResult.User;
                if (AccountConfig.RequireEmailConfirmation && model.Provider != "Google")
                    await SendUserConfirmationEmail(user!);
                var userLogin = new UserLoginInfo(model.Provider, model.ProviderId, null);
                var addLoginIdentityResult =
                    await _signInManager.UserManager.AddLoginAsync(user!, userLogin);
                if (addLoginIdentityResult.Succeeded)
                {
                    var additionalClaims = new List<Claim>();
                    if (!string.IsNullOrWhiteSpace(model.SessionId))
                        additionalClaims.Add(new Claim(JwtClaimTypes.SessionId, model.SessionId));
                    additionalClaims.Add(new Claim("name", model.Username));
                    var props = new AuthenticationProperties();
                    if (!string.IsNullOrWhiteSpace(model.IdToken))
                        props.StoreTokens(
                            new[] { new AuthenticationToken { Name = "id_token", Value = model.IdToken } }
                        );
                    var issuer = new IdentityServerUser(user!.Id.ToString())
                    {
                        DisplayName = user!.UserName,
                        IdentityProvider = model.Provider,
                        AdditionalClaims = additionalClaims
                    };
                    await HttpContext.SignInAsync(issuer, props);
                    await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
                    var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                    if (context != null)
                        await _events.RaiseAsync(new UserLoginSuccessEvent(
                            model.Provider, model.ProviderId, user!.Id.ToString(),
                            user!.UserName, true, context.Client.ClientId)
                        );
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    throw new InvalidOperationException($"\"{model.ReturnUrl}\" is  invalid");
                }
                else
                {
                    ModelState.AddModelError("SignUp-Error", "Something went wrong!");
                }
            }
            foreach (var error in createUserResult.Errors)
                ModelState.AddModelError("SignUp-Error", error.Description);
            return View();
        }

        [HttpGet("/Auth/Confirmation/{email}")]
        public async Task<IActionResult> ConfirmEmail(string useId, string token)
        {
            if (useId == null || token == null )
            {
                return RedirectToAction("signIn","authentication");
            }
            var user = await _signInManager.UserManager.FindByEmailAsync(useId);
            if (user == null)
            {
                ModelState.AddModelError("ConfirmEmail-Error",$"The email {useId} in Valid");
                return View();
            }
            var result = await _signInManager.UserManager.ConfirmEmailAsync(user, token);
            if(result.Succeeded)
            {
                return View();
            }
            ModelState.AddModelError("ConfirmEmail-Error", "Email cannot be confirmed");
            return View();
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

        private static Task<MailRequest> GenerateEmailAsync(string receiver, string token)
        {
            var body = "Thanks for your registration," +
                $" this is your email confirmation <a href=\"{$"https://localhost:7265/auth/confirmation/{receiver}?token={token}"}\">link</a>" +
                $" The link will be expired at {DateTime.UtcNow.AddMinutes(30):dddd, MMMM d, yyyy; HH:mm:ss tt}";
            return Task.FromResult<MailRequest>(new()
            {
                Body = body,
                Sender = "gigamallservice@gmail.com",
                IsHtmlMessage = true,
                Receiver = receiver,
                Subject = "Email confirmation"
            });
        }

        
    }
}

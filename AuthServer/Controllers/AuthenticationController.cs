﻿using AuthServer.Abstractions;
using AuthServer.Configurations;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthServer.Controllers
{
    [ServiceFilter(typeof(SignInActionFilter))]
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

        [AllowAnonymous]
        [Route("/auth/SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignIn-Error", "Username or password is invalid");
                return View(model);
            }
            var user = await _signInManager.UserManager.FindByNameAsync(model.Username);
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, AccountConfig.AccountLockedOutEnabled);
            if (user != null && signInResult.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                throw new InvalidOperationException($"\"{model.ReturnUrl}\" is  invalid");
            }
            else if (user != null && signInResult.IsLockedOut)
            {
                ModelState.AddModelError("SignIn-Error", "Account is locked out");
                ViewBag.LockedOutCancelTime = user!.LockoutEnd!.Value;
                return View(model);
            }
            ModelState.AddModelError("SignIn-Error", "Username and/or password is incorrect");
            return View(model);
        }

        [AllowAnonymous]
        [Route("/auth/SignUp")]
        public IActionResult SignUp()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/SignUp")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignUp-Error", "Input information is invalid");
                return View(model);
            }
            var createUserResult = await CreateUserAsync(model);
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

        [AllowAnonymous]
        [HttpPost]
        [Route("/auth/ExternalConfirmation")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalSignInCreateAccount(ExternalSignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("SignUp-Error", "Input information is invalid");
                return View(model);
            }
            var createUserResult = 
                await CreateUserAsync(model);
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

        private async Task<CreateUserResult> CreateUserAsync(SignUpModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                if (user.UserName == model.Username)
                    return CreateUserResult.Failed(new IdentityError
                    {
                        Code = "UsernameIsAlreadyExisted",
                        Description = "This username is in used"
                    });
                return CreateUserResult.Failed(new IdentityError
                {
                    Code = "EmailIsInUsed",
                    Description = "This email is linked to another account"
                });
            }
            user = new User
            {
                Email = model.Email,
                UserName = model.Username,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Username.ToUpper(),
                DoB = model.DoB,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var createAccountResult = await _signInManager.UserManager.CreateAsync(user, model.Password);
            if (createAccountResult.Succeeded)
            {
                await _signInManager.UserManager.AddClaimsAsync(user, new[]
                {
                    new Claim(ClaimTypes.Email, model.Email)
                });
            }
            return CreateUserResult.Success(user);
        }
    }
}

using AspNetCoreSharedComponent.Mail;
using AuthServer.Extensions;
using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Contexts;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AdditionApiController : ControllerBase
    {
        private readonly ApplicationUserManager _userManager;

        private readonly IMailService _mailer;

        public AdditionApiController(ApplicationUserManager userManager, IMailService mailer)
        {
            _userManager = userManager;
            _mailer = mailer;
        }

        [HttpPost("shop-owner-register")]
        public async Task<ApiResult> ShopOwnerRegister([FromForm] ShopOwnerSignUpModel model)
        {
            var count = 0;
            var username = GenerateShopOwnerUsername(model, count++);
            var password = GenerateShopOwnerPassword();
            var internalCreateUserResult = await _userManager.CreateUserAsync(model, username, password);
            while (!internalCreateUserResult.Succeeded)
            {
                if (internalCreateUserResult.Errors.Any(error => error.Code.Contains("password", StringComparison.CurrentCultureIgnoreCase)))
                {
                    password = GenerateShopOwnerPassword();
                }
                else
                {
                    username = GenerateShopOwnerUsername(model, count);
                }
                internalCreateUserResult = await _userManager.CreateUserAsync(model, username, password);
            }
            _mailer.SendMail(new MailRequest
            {
                Sender = "gigamallservice@gmail.com",
                Receiver = model.Email,
                IsHtmlMessage = false,
                Subject = "Shop owner password",
                Body = $"This is your password, keep it carefully: {password}"
            });
            return ApiResult<CreateShopOwnerAccountResult>
                .CreateSucceedResult(new CreateShopOwnerAccountResult(internalCreateUserResult.User!.Id, username));
        }

        [NonAction]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        private static string GenerateShopOwnerUsername(ShopOwnerSignUpModel model, int count)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (model.FirstName == null)
                throw new ArgumentNullException(nameof(model.FirstName));
            if (model.LastName == null)
                throw new ArgumentNullException(nameof(model.LastName));
            return string.Join("", model.FirstName.ToLower().Split(" ").Select(name => name[0])) + model.LastName.ToLower() + (count <= 0 ? "" : count);
        }
        
        [NonAction]
        private static string GenerateShopOwnerPassword()
        {
            var acceptedChars = Enumerable.Range(48, 10).Concat(Enumerable.Range(65, 26))
                    .Concat(Enumerable.Range(97, 26)).Append(64)
                    .Select(code => (char) code).ToArray();
            var acceptedCharsLength = acceptedChars.Length;
            var length = 8;
            var passwordStringBuilder = new StringBuilder();
            var random = new Random();
            for (int i = 1; i <= length; i++)
            {
                passwordStringBuilder.Append(acceptedChars[random.Next(acceptedCharsLength)]);
            }
            return passwordStringBuilder.ToString();
        }
    }
}

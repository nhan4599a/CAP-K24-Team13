using AuthServer.Commands;
using AuthServer.Extensions;
using AuthServer.Identities;
using AuthServer.Models;
using DatabaseAccessor.Contexts;
using DatabaseAccessor.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace AuthServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [ApiController]
    [Route("/api")]
    public class AdditionApiController : ControllerBase
    {
        private readonly ApplicationUserManager _userManager;

        private readonly ApplicationDbContext _dbContext;

        private readonly IMailService _mailer;

        private readonly IMediator _mediator;

        public AdditionApiController(ApplicationUserManager userManager,
            ApplicationDbContext dbContext, IMailService mailer, IMediator mediator)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mailer = mailer;
            _mediator = mediator;
        }

        [HttpPost("shop-owner-register")]
        public async Task<ApiResult> ShopOwnerRegister(ShopOwnerSignUpModel model)
        {
            var count = 0;
            var username = GenerateShopOwnerUsername(model, count++);
            var password = GenerateShopOwnerPassword();
            var internalCreateUserResult = await _userManager.CreateUserAsync(model, username, password);
            while (!internalCreateUserResult.Succeeded)
            {
                username = GenerateShopOwnerUsername(model, count);
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

        [HttpGet("report")]
        public async Task<ApiResult> GetAllReport([FromQuery] PaginationInfo paginationInfo)
        {
            var result = await _mediator.Send(new GetAllReportsQuery
            {
                PaginationInfo = paginationInfo
            });
            return ApiResult<PaginatedList<ReportDTO>>.CreateSucceedResult(result);
        }

        [HttpPost("report/{reportId}")]
        public async Task<ApiResult> ApproveReport(int reportId)
        {
            var result = await _mediator.Send(new ApproveReportCommand
            {
                ReportId = reportId
            });
            if (result.IsSuccess)
                return ApiResult.CreateErrorResult(400, result.ErrorMessage);
            if (result.Response.Item2 != AccountPunishmentBehavior.SendAlertEmail)
            {
                DateTimeOffset? dateTimeOffset = result.Response.Item2 == AccountPunishmentBehavior.LockedOutPermanently
                    ? null : DateTimeOffset.Now.AddDays(14);
                _dbContext.Attach(result.Response.Item1);
                await _userManager.SetLockoutEndDateAsync(result.Response.Item1, dateTimeOffset);
                result.Response.Item1.Status = AccountStatus.Banned;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _mailer.SendMail(new MailRequest
                {
                    Sender = "gigamallservice@gmail.com",
                    Receiver = result.Response.Item1.Email,
                    IsHtmlMessage = false,
                    Subject = "Alert",
                    Body = $"Look like you are reported. Your account will be locked out in 14 days if you violated rule again." +
                    $" If you continue after punished, your account will be ban permanently"
                });
            }
            return ApiResult.SucceedResult;
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

using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading.Tasks;
using UserService.Background;
using UserService.Commands;

namespace UserService.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IBackgroundJobClient _backgroundJobs;

        public UserController(IMediator mediator, IBackgroundJobClient backgroundJobs)
        {
            _mediator = mediator;
            _backgroundJobs = backgroundJobs;
        }

        [HttpGet]
        public async Task<ApiResult> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return ApiResult<PaginatedList<UserDTO>>.CreateSucceedResult(result);
        }

        [HttpPost("ban/{userId}")]
        public async Task<ApiResult> ApplyBan(string userId, [FromBody] int behaviorInt)
        {
            var behavior = (AccountPunishmentBehavior)behaviorInt;
            var parseResult = Guid.TryParse(userId, out Guid parsedUserId);
            if (!parseResult)
                return ApiResult.CreateErrorResult(400, "userId is invalid");
            if (behavior == AccountPunishmentBehavior.SendAlertEmail)
            {
                _backgroundJobs.Enqueue<SendAlertEmailBackgroundJob>(job => job.SendEmail(userId));
                return ApiResult.SucceedResult;
            }
            var response = await _mediator.Send(new BanUserCommand
            {
                UserId = parsedUserId,
                Behavior = behavior
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult.SucceedResult;
        }
    }
}
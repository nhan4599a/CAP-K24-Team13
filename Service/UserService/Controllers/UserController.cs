using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Threading.Tasks;
using UserService.Commands;

namespace UserService.Controllers
{
    [ApiController]
    [Route("/api/users")]
    [Authorize(Roles = $"{SystemConstant.Roles.ADMIN_TEAM_5}, {SystemConstant.Roles.ADMIN_TEAM_13}")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ApiResult> FindUsers([FromQuery] FindUsersQuery query)
        {
            var result = await _mediator.Send(query);
            return ApiResult<PaginatedList<UserDTO>>.CreateSucceedResult(result);
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<ApiResult> GetUser(string userId)
        {
            var parseResult = Guid.TryParse(userId, out Guid parsedUserId);
            if (!parseResult)
                return ApiResult.CreateErrorResult(400, "UserId is invalid");
            var result = await _mediator.Send(new GetUserByIdQuery
            {
                UserId = parsedUserId
            });
            if (result == null)
                return ApiResult.CreateErrorResult(404, "User not found");
            return ApiResult<UserDTO>.CreateSucceedResult(result);
        }

        [HttpPost("ban/{userId}")]
        public async Task<ApiResult> ApplyBan(string userId, [FromForm] uint? dayCount)
        {
            var parseResult = Guid.TryParse(userId, out Guid parsedUserId);
            if (!parseResult)
                return ApiResult.CreateErrorResult(400, "UserId is invalid");
            if (dayCount != null && dayCount == 0)
                return ApiResult.CreateErrorResult(400, "Day count must be greater than zero");
            var response = await _mediator.Send(new BanUserCommand
            {
                UserId = parsedUserId,
                DayCount = dayCount
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult.SucceedResult;
        }

        [HttpPost("unban/{userId}")]
        public async Task<ApiResult> Unban(string userId)
        {
            var parseResult = Guid.TryParse(userId, out Guid parsedUserId);
            if (!parseResult)
                return ApiResult.CreateErrorResult(400, "UserId is invalid");
            var response = await _mediator.Send(new UnbanUserCommand
            {
                UserId = parsedUserId
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult.SucceedResult;
        }

        [HttpPost("{userId}/assign/{shopId}")]
        public async Task<ApiResult> AssignToShopOwner(string userId, int shopId)
        {
            var parseResult = Guid.TryParse(userId, out Guid parsedUserId);
            if (!parseResult)
                return ApiResult.CreateErrorResult(400, "UserId is invalid");
            var response = await _mediator.Send(new AssignToShopOwnerCommand
            {
                UserId = parsedUserId,
                ShopId = shopId
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(500, response.ErrorMessage);
            return ApiResult.SucceedResult;
        }
    }
}
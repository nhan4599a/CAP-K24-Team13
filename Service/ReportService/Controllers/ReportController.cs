using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportService.Clients;
using ReportService.Commands;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReportService.Controllers
{
    [ApiController]
    [Route("/api/reports")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IUserClient _userClient;

        public ReportController(IMediator mediator, IUserClient userClient)
        {
            _mediator = mediator;
            _userClient = userClient;
        }

        [HttpPost("{invoiceId}")]
        public async Task<ApiResult> CreateReport(int invoiceId)
        {
            var reporterString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parseResult = Guid.TryParse(reporterString, out Guid reporter);
            if (!parseResult)
                return ApiResult.CreateErrorResult(400, "can not determine who are you");
            var response = await _mediator.Send(new CreateReportCommand
            {
                InvoiceId = invoiceId,
                ReporterId = reporter
            });
            if (!response.IsSuccess)
                return ApiResult.CreateErrorResult(400, response.ErrorMessage);
            return ApiResult.SucceedResult;
        }

        [HttpPost("approve/{reportId}")]
        public async Task<ApiResult> ApproveReport(int reportId)
        {
            var accessToken = Request.Headers.Authorization.ToString();
            if (!accessToken.StartsWith("Bearer "))
            {
                return ApiResult.CreateErrorResult(401, "Access token is invalid");
            }
            var result = await _mediator.Send(new ApproveReportCommand
            {
                ReportId = reportId
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            //try
            //{
            //}
            //catch (Exception e)
            //{
            //    return ApiResult.CreateErrorResult(500, e.Message);
            //}

            var response = await _userClient.ApplyBan(accessToken.Split(" ")[1], result.Response.Item1, result.Response.Item2);
            if (!response.IsSuccessStatusCode || response.Content.ResponseCode != 200)
            {
                return ApiResult.CreateErrorResult(500, response.Content?.ErrorMessage);
            }
            return ApiResult.SucceedResult;
        }

        [HttpGet]
        public async Task<ApiResult> GetAllReports([FromQuery] PaginationInfo paginationInfo)
        {
            var request = new GetAllReportsQuery { PaginationInfo = paginationInfo };
            var response = await _mediator.Send(request);
            return ApiResult<PaginatedList<ReportDTO>>.CreateSucceedResult(response);
        }
    }
}
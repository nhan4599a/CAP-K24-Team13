using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportService.Clients;
using ReportService.Commands;
using Shared.DTOs;
using Shared.Models;
using System;
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
        public async Task<ApiResult> CreateReport(int invoiceId, [FromBody] Guid reporter)
        {
            var response = await _mediator.Send(new CreateReportCommand
            {
                InvoiceId = invoiceId,
                ReporterId = reporter
            });
            if (response.IsSuccess)
                return ApiResult.CreateErrorResult(400, response.ErrorMessage);
            return ApiResult.SucceedResult;
        }

        [HttpPost("approve/{reportId}")]
        public async Task<ApiResult> ApproveReport([FromHeader] string accessToken, int reportId)
        {
            var result = await _mediator.Send(new ApproveReportCommand
            {
                ReportId = reportId
            });
            if (!result.IsSuccess)
                return ApiResult.CreateErrorResult(500, result.ErrorMessage);
            if (!accessToken.StartsWith("Bearer "))
            {
                return ApiResult.CreateErrorResult(500, "Access token is invalid");
            }
            try
            {
                await _userClient.ApplyBan(accessToken.Split(" ")[1], result.Response.Item1, result.Response.Item2);
                return ApiResult.SucceedResult;
            }
            catch (Exception e)
            {
                return ApiResult.CreateErrorResult(500, e.Message);
            }
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
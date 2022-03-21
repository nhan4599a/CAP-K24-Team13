using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportService.Commands;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace ReportService.Controllers
{
    [ApiController]
    [Route("/api/report")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
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
    }
}
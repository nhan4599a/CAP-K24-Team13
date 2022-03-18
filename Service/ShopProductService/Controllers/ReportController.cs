using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using ShopProductService.Commands.Report;
using System;
using System.Threading.Tasks;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("Report")]
    public class ReportController : Controller
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("GetSaleByDate")]
        [HttpGet]
        public async Task<ApiResult> GetSaleByDate(DateTime date, int shopId)
        {
            var result = await _mediator.Send(new GetReportByDateQuery { Date = date, ShopId = shopId });
            return ApiResult<SaleReportDTO>.CreateSucceedResult(result);
        }

        [Route("GetSaleByMonth")]
        [HttpGet]
        public async Task<ApiResult> GetSaleByMonth(DateTime date, int shopId)
        {
            var result = await _mediator.Send(new GetReportByMonthQuery { Month = date, ShopId = shopId });
            return ApiResult<SaleReportDTO>.CreateSucceedResult(result);
        }
    }
}

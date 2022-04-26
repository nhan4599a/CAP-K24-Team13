using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Models;
using StatisticService.Commands;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StatisticService.Controllers
{
    [ApiController]
    [Route("/api/statistic")]
    [Authorize]
    public class StatisticController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IDistributedCache _cache;

        private static readonly DistributedCacheEntryOptions cacheOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

        public StatisticController(IMediator mediator, IDistributedCache cache)
        {
            _mediator = mediator;
            _cache = cache;
        }

        [HttpGet("shop/{shopId}/orders")]
        public async Task<ApiResult> StatisticOrder(int shopId,
            [FromQuery(Name = "strategy")] StatisticStrategy strategy,
            [FromQuery(Name = "start")] string start,
            [FromQuery(Name = "end")] string end)
        {
            var parseResult = StatisticDateRange.TryCreate(strategy, start, end, out StatisticDateRange range);
            if (!parseResult.IsSucceed)
                return ApiResult.CreateErrorResult(400, parseResult.Exception.Message);
            var cachedResult = _cache.GetString(GetCacheKey(strategy, range));
            if (cachedResult != null)
            {
                return ApiResult<StatisticResult>.CreateSucceedResult(JsonSerializer.Deserialize<StatisticResult>(cachedResult)!);
            }
            try
            {
                var result = await _mediator.Send(new OrderStatisticCommand
                {
                    ShopId = shopId,
                    Strategy = strategy,
                    Range = range
                });
                result.Key = GetCacheKey(strategy, range);
                await _cache.SetStringAsync(result.Key, JsonSerializer.Serialize(result), cacheOptions);
                return ApiResult<StatisticResult>.CreateSucceedResult(result);
            }
            catch (Exception e)
            {
                return ApiResult.CreateErrorResult(500, e.Message);
            }
        }

        [HttpGet("get/{key}")]
        public async Task<IActionResult> GetStatistic(string key)
        {
            var cachedResult = await _cache.GetStringAsync(key);
            if (cachedResult == null)
                return StatusCode(StatusCodes.Status404NotFound);
            var parsedCacheResult = JsonSerializer.Deserialize<StatisticResult>(cachedResult);
            var columnCount = parsedCacheResult!.Details.Count;
            using var workbook = new XLWorkbook();
            var incomeSheet = workbook.Worksheets.Add("Income");
            var numberOfInvoicesSheet = workbook.Worksheets.Add("Number of invoices");
            incomeSheet.Cell("B1").Value = $"{parsedCacheResult.StatisticBy.GetStrategy()} from {parsedCacheResult.Details.First().Key} to {parsedCacheResult.Details.Last().Key}";
            numberOfInvoicesSheet.Cell("B1").Value = $"{parsedCacheResult.StatisticBy.GetStrategy()} from {parsedCacheResult.Details.First().Key} to {parsedCacheResult.Details.Last().Key}";
            incomeSheet.Range(1, 2, 1, 1 + columnCount).Merge();
            numberOfInvoicesSheet.Range(1, 2, 1, 1 + columnCount).Merge();
            incomeSheet.Range(1, 2, 1, 1 + columnCount).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            numberOfInvoicesSheet.Range(1, 2, 1, 1 + columnCount).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            incomeSheet.Cell("A3").Value = "Income";
            numberOfInvoicesSheet.Cell("A3").Value = "New Orders";
            numberOfInvoicesSheet.Cell("A4").Value = "Succeed Orders";
            numberOfInvoicesSheet.Cell("A5").Value = "Canceled Orders";
            var currentColumn = 2;
            foreach (var value in parsedCacheResult.Details)
            {
                incomeSheet.Cell(2, currentColumn).SetValue(value.Key);
                numberOfInvoicesSheet.Cell(2, currentColumn).SetValue(value.Key);
                if (value.Value == null)
                {
                    incomeSheet.Cell(3, currentColumn).Value = 0;

                    numberOfInvoicesSheet.Cell(3, currentColumn).Value = 0;
                    numberOfInvoicesSheet.Cell(4, currentColumn).Value = 0;
                    numberOfInvoicesSheet.Cell(5, currentColumn).Value = 0;
                }
                else
                {
                    incomeSheet.Cell(3, currentColumn).Value = value.Value.Income;

                    numberOfInvoicesSheet.Cell(3, currentColumn).Value = value.Value.Data.NewInvoiceCount;
                    numberOfInvoicesSheet.Cell(4, currentColumn).Value = value.Value.Data.SucceedInvoiceCount;
                    numberOfInvoicesSheet.Cell(5, currentColumn).Value = value.Value.Data.CanceledInvoiceCount;
                }

                currentColumn += 1;
            }
            incomeSheet.Columns().AdjustToContents();
            numberOfInvoicesSheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "statistic.xlsx");
        }

        private static string GetCacheKey(StatisticStrategy strategy, StatisticDateRange range)
        {
            return $"Statistic.{strategy}.{range.Range.Start:ddMMyyyy}-{range.Range.End:ddMMyyyy}";
        }
    }
}

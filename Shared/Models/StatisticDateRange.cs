using Shared.Extensions;
using System;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticDateRange
    {
        [JsonIgnore]
        public StatisticStrategy Strategy { get; set; }

        public Result Range { get; init; }

        public StatisticDateRange(StatisticStrategy strategy, string start, string end)
        {
            if (strategy == StatisticStrategy.ByDay)
            {
                if (DateTimeExtension.TryParseExact(start, "dd/MM/yyyy", out DateTime startDate) &&
                        DateTimeExtension.TryParseExact(end, "dd/MM/yyyy", out DateTime endDate))
                {
                    Range = new Result(startDate, endDate.AddDays(1).AddSeconds(-1));
                    return;
                }
            }
            else if (strategy == StatisticStrategy.ByMonth)
            {
                if (DateTimeExtension.TryParseExact(start, "MM/yyyy", out DateTime startDate) &&
                        DateTimeExtension.TryParseExact(end, "MM/yyyy", out DateTime endDate))
                {
                    Range = new Result(startDate, DateTimeExtension.EndOfMonth(endDate.Month, endDate.Year));
                    return;
                }
            }
            else if (strategy == StatisticStrategy.ByQuarter)
            {
                if (DateTimeExtension.TryParseExact(start, "MM/yyyy", out DateTime startDate) &&
                        DateTimeExtension.TryParseExact(end, "MM/yyyy", out DateTime endDate))
                {
                    if (startDate.Month < 1 || startDate.Month > 4 || endDate.Month < 1 || endDate.Month > 4)
                        throw new ArgumentException("Provided range is not a valid quarter");
                    int startMonth = startDate.Month * 3 - 2;
                    int endMonth = endDate.Month * 3;
                    Range = new Result(DateTimeExtension.StartOfMonth(startMonth, startDate.Year),
                        DateTimeExtension.EndOfMonth(endMonth, endDate.Year));
                    return;
                }
            }
            else if (strategy == StatisticStrategy.ByYear)
            {
                if (DateTimeExtension.TryParseExact(start, "yyyy", out DateTime startDate) &&
                        DateTimeExtension.TryParseExact(end, "yyyy", out DateTime endDate))
                {
                    Range = new Result(DateTimeExtension.StartOfYear(startDate.Year),
                        DateTimeExtension.EndOfYear(endDate.Year));
                    return;
                }
            }
        }

        public static ParseResult TryCreate(StatisticStrategy strategy, string start, string end, out StatisticDateRange range)
        {
            if (string.IsNullOrWhiteSpace(end))
                end = new StatisticDateResult(strategy, DateTime.Now).ToString();
            try
            {
                range = new StatisticDateRange(strategy, start, end);
                return ParseResult.SucceedResult;
            }
            catch (Exception e)
            {
                range = null;
                return ParseResult.CreateErrorResult(e);
            }
        }

        public class Result
        {
            public DateTime Start { get; set; }

            public DateTime End { get; set; }

            public Result(DateTime start, DateTime end)
            {
                if (end < start)
                    throw new ArgumentException($"{nameof(start)} must be less than or equal to {nameof(end)}");
                Start = start;
                End = end;
            }
        }

        public class ParseResult
        {
            public bool IsSucceed { get; init; }

            public Exception Exception { get; init; }

            private ParseResult() { }


            public readonly static ParseResult SucceedResult = new()
            {
                IsSucceed = true
            };

            public static ParseResult CreateErrorResult(Exception e)
            {
                return new()
                {
                    IsSucceed = false,
                    Exception = e
                };
            }
        }
    }
}

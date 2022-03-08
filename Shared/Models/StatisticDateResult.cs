using System;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticDateResult : IComparable<StatisticDateResult>
    {
        [JsonIgnore]
        public StatisticStrategy DateResultType { get; set; }

        public DateOnly Result { get; set; }

        public StatisticDateResult(StatisticStrategy strategy, DateOnly dateOnly)
        {
            DateResultType = strategy;
            Result = dateOnly;
        }

        public override string ToString()
        {
            string format = DateResultType == StatisticStrategy.ByDay ? "dd/MM/yyyy" : "MM/yyyy";
            return Result.ToString(format);
        }

        public override int GetHashCode()
        {
            return Result.GetHashCode();
        }

        public int CompareTo(StatisticDateResult obj)
        {
            return Result.CompareTo(obj.Result);
        }

        public static explicit operator string(StatisticDateResult statisticDateResult) => statisticDateResult.ToString();
    }
}

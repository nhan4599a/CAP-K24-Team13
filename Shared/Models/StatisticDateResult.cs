using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticDateResult : EqualityComparer<StatisticDateResult>, IComparable<StatisticDateResult>
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

        public override bool Equals(StatisticDateResult x, StatisticDateResult y)
        {
            return x.CompareTo(y) == 0;
        }

        public override int GetHashCode([DisallowNull] StatisticDateResult obj)
        {
            return obj.GetHashCode();
        }
    }
}

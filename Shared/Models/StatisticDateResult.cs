using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticDateResult : IComparable<StatisticDateResult>
    {
        [JsonIgnore]
        public StatisticStrategy Strategy { get; set; }

        public DateTime Result { get; set; }

        public static Comparer DefaultComparer => new();

        public StatisticDateResult(StatisticStrategy strategy, DateTime result)
        {
            Strategy = strategy;
            Result = result;
        }

        public override string ToString()
        {
            string format = Strategy switch
            {
                StatisticStrategy.ByDay => "dd/MM/yyyy",
                StatisticStrategy.ByMonth => "MM/yyyy",
                StatisticStrategy.ByQuarter => "M/yyyy",
                StatisticStrategy.ByYear => "yyyy",
                _ => throw new NotImplementedException()
            };
            return Result.ToString(format);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(StatisticDateResult obj)
        {
            return Result.CompareTo(obj.Result);
        }

        public class Comparer : IComparer<StatisticDateResult>
        {
            public int Compare(StatisticDateResult x, StatisticDateResult y)
            {
                return x.CompareTo(y);
            }
        }
    }
}

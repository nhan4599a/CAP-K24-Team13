﻿using System;
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

        public StatisticDateResult(StatisticStrategy strategy, DateTime dateOnly)
        {
            Strategy = strategy;
            Result = dateOnly;
        }

        public override string ToString()
        {
            string format = Strategy == StatisticStrategy.ByDay ? "dd/MM/yyyy" :
                (Strategy == StatisticStrategy.ByYear ? "yyyy" : "MM/yyyy");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessor.Converters
{
    public static class DateOnlyExtensions
    {
        public static DateTime ToDateTime(this DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);

        public static DateOnly FromDateTime(this DateTime date) => DateOnly.FromDateTime(date);

        public static DateTime? ToDateTime(this DateOnly? dateOnly) => dateOnly.HasValue ? dateOnly.Value.ToDateTime() : null;

        public static DateOnly? FromDateTime(this DateTime? date) => date.HasValue ? date.Value.FromDateTime() : null;
    }
}

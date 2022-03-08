using System;

namespace DatabaseAccessor.Converters
{
    public static class DateOnlyExtensions
    {
        public static DateTime ToDateTime(this DateOnly dateOnly) => dateOnly.ToDateTime(TimeOnly.MinValue);

        public static DateOnly ToDateOnly(this DateTime date) => DateOnly.FromDateTime(date);

        public static DateTime? ToDateTime(this DateOnly? dateOnly) => dateOnly.HasValue ? dateOnly.Value.ToDateTime() : null;

        public static DateOnly? ToDateTime(this DateTime? date) => date.HasValue ? date.Value.ToDateOnly() : null;
    }
}

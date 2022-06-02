using System;
using System.Globalization;

namespace Shared.Extensions
{
    public class DateTimeExtension
    {
        public static bool TryParseExact(string value, string format, out DateTime dateTime)
        {
            return DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
        }

        public static DateTime StartOfMonth(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException($"{month} is not a valid month");
            return new DateTime(year, month, 1);
        }

        public static DateTime EndOfMonth(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException($"{month} is not a valid month");
            return new DateTime(year, month, DayOfMonth(month, year), 23, 59, 59);
        }

        public static DateTime StartOfYear(int year)
        {
            return new DateTime(year, 1, 1);
        }

        public static DateTime EndOfYear(int year)
        {
            return EndOfMonth(12, year);
        }

        public static int GetQuarter(int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException($"{month} is not a valid month");
            return (int)Math.Ceiling(month / 3d);
        }

        public static int DayOfMonth(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException($"{month} is not a valid month");
            return month <= 7 ?
                (month == 2 ? (DateTime.IsLeapYear(year) ? 29 : 28) : (month % 2 == 1 ? 31 : 30)) :
                (month % 2 == 1 ? 30 : 31);
        }
    }
}

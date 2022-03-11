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
    }
}

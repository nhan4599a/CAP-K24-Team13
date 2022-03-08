using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace DatabaseAccessor.Converters
{
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(dateOnly => dateOnly.ToDateTime(), dateTime => dateTime.ToDateOnly())
        { }
    }
}

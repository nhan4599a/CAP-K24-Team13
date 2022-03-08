using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace DatabaseAccessor.Converters
{
    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        public NullableDateOnlyConverter() : base(dateOnly => dateOnly.ToDateTime(), dateTime => dateTime.ToDateTime()) 
        { }
    }
}

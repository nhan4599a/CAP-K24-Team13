using Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreSharedComponent.JSON
{
    public class DictionaryKeyNonStringConverter<TKey> : JsonConverter<IDictionary<TKey, object>> where TKey : IConvertable<TKey>
    {
        public override IDictionary<TKey, object>? Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<TKey, object> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

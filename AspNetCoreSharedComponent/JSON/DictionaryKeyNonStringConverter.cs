﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreSharedComponent.JSON
{
    public class DictionaryKeyNonStringConverter<TKey> : JsonConverter<IDictionary<TKey, object>>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType) return false;
            if (typeToConvert.GenericTypeArguments[0] == typeof(string)) return false;
            return typeToConvert.GetInterface("IDictionary") != null;
        }

        public override IDictionary<TKey, object>? Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<TKey, object> value, JsonSerializerOptions options)
        {
            var convertedDictionary = new Dictionary<string, object>(value.Count);
            foreach (var (k, v) in value) convertedDictionary[k!.ToString()!] = v;
            JsonSerializer.Serialize(writer, convertedDictionary, options);
            convertedDictionary.Clear();
        }
    }
}

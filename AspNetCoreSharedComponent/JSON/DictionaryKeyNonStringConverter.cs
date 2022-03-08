using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AspNetCoreSharedComponent.JSON
{
    public class DictionaryKeyNonStringConverter<TKey> : JsonConverter<IDictionary<TKey, object>>
    {
        private readonly ILogger _logger;

        public DictionaryKeyNonStringConverter(ILogger logger)
        {
            _logger = logger;
        }
            
        public override IDictionary<TKey, object>? Read(ref Utf8JsonReader reader,
            Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IDictionary<TKey, object> value, JsonSerializerOptions options)
        {
            _logger.LogInformation("Writing json");
            var convertedDictionary = new Dictionary<string, object>(value.Count);
            foreach (var (k, v) in value) convertedDictionary[k!.ToString()!] = v;
            JsonSerializer.Serialize(writer, convertedDictionary, options);
            convertedDictionary.Clear();
        }

        public class Factory : JsonConverterFactory
        {
            private readonly ILogger _logger;

            public Factory(ILogger logger)
            {
                _logger = logger;
            }

            public override bool CanConvert(Type typeToConvert)
            {
                if (!typeToConvert.IsGenericType) return false;
                if (typeToConvert.GenericTypeArguments[0] == typeof(string)) return false;
                var @interface = typeToConvert.GetInterface("IDictionary");
                return @interface != null;
            }

            public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            {
                var converterType = typeof(DictionaryKeyNonStringConverter<>)
                   .MakeGenericType(typeToConvert.GenericTypeArguments[0]);
                var converter = (JsonConverter)Activator.CreateInstance(
                    converterType,
                    BindingFlags.Instance | BindingFlags.Public,
                    null,
                    _logger,
                    CultureInfo.CurrentCulture)!;
                return converter;
            }
        }
    }
}

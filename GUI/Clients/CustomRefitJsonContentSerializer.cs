using Refit;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace GUI.Clients
{
    public class CustomRefitJsonContentSerializer : IHttpContentSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public CustomRefitJsonContentSerializer() : this(GetDefaultJsonSerializerOptions())
        { }

        public CustomRefitJsonContentSerializer(JsonSerializerOptions jsonSerializerOptions)
        {
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
        {
            var item = await content.ReadFromJsonAsync<T>(_jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
            return item;
        }

        public string GetFieldNameForProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));

            return propertyInfo.GetCustomAttributes<JsonPropertyNameAttribute>(true)
                       .Select(a => a.Name)
                       .FirstOrDefault();
        }

        public HttpContent ToHttpContent<T>(T item)
        {
            var content = JsonContent.Create(item, options: _jsonSerializerOptions);

            return content;
        }

        public static JsonSerializerOptions GetDefaultJsonSerializerOptions()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                // Default to case insensitive property name matching as that's likely the behavior most users expect
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            jsonSerializerOptions.Converters.Add(new BooleanTypeConverter());
            jsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return jsonSerializerOptions;
        }
    }

    public class BooleanTypeConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.True)
                return true;
            if (reader.TokenType == JsonTokenType.False)
                return false;
            if (reader.TokenType == JsonTokenType.String)
            {
                var strValue = reader.GetString()!.ToLower();

                return strValue == "true";
            }
            if (reader.TokenType == JsonTokenType.Number)
            {

                var byteValue = reader.GetByte();

                return byteValue > 0;
            }

            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}

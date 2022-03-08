using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSharedComponent.JSON
{
    public static class MvcJsonExtension
    {
        public static IMvcBuilder AddJsonPropertyToStringSerializer<T>(this IMvcBuilder builder, ILogger logger)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Insert(0, new JsonPropertyToStringConverter<T>());
                options.JsonSerializerOptions.Converters.Add(new DictionaryKeyNonStringConverter<T>.Factory(logger));
            });
            return builder;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreSharedComponent.JSON
{
    public static class MvcJsonExtension
    {
        public static IMvcBuilder AddJsonPropertyToStringSerializer<T>(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Insert(0, new JsonPropertyToStringConverter<T>());
                options.JsonSerializerOptions.Converters.Insert(0, new DictionaryKeyNonStringConverter<T>.Factory());
            });
            return builder;
        }
    }
}

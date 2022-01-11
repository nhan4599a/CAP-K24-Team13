using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreSharedComponent.ModelBinders.Providers
{
    public class StringToDateOnlyModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (context.Metadata.ModelType == typeof(string))
                return new StringToDateOnlyModelBinder();
            return null;
        }
    }
}

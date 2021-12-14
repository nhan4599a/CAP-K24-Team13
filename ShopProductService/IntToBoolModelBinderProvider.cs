using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace ShopProductService
{
    public class IntToBoolModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (context.Metadata.ModelType == typeof(int))
                return new IntToBoolModelBinder();
            return null;
        }
    }
}

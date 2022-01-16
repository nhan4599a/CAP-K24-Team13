using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreSharedComponent.ModelBinders
{
    public class StringToDateOnlyModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var value = valueProviderResult.FirstValue;
            if (DateOnly.TryParseExact(value, "dd/MM/yyyy", out DateOnly dateOnly))
                bindingContext.Result = ModelBindingResult.Success(dateOnly);
            else
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, 
                    $"{bindingContext.ModelName} must be in \"dd/MM/yyyy\" format");
            return Task.CompletedTask;
        }
    }
}

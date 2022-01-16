using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreSharedComponent.ModelBinders
{
    public class IntToBoolModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var value = valueProviderResult.FirstValue;
            if (int.TryParse(value, out int intResult))
                bindingContext.Result = ModelBindingResult.Success(intResult != 0);
            else if (bool.TryParse(value, out bool boolResult))
                bindingContext.Result = ModelBindingResult.Success(boolResult);
            else
                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    $"{bindingContext.ModelName} must be int or bool value");
            return Task.CompletedTask;
        }
    }
}

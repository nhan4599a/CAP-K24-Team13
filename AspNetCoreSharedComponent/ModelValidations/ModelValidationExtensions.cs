using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace AspNetCoreSharedComponent.ModelValidations
{
    public static class ModelValidationExtensions
    {
        public static IMvcBuilder AddFluentValidation<TModel, TValidator>(this IMvcBuilder builder)
            where TValidator : AbstractValidator<TModel>
        {
            if (!builder.Services.Any(s => s.Lifetime == ServiceLifetime.Singleton 
                && s.ImplementationType == typeof(ValidatorConfiguration)))
            {
                builder.AddFluentValidation();
            }
            builder.Services.AddTransient<IValidator<TModel>, TValidator>();
            return builder;
        }
    }
}

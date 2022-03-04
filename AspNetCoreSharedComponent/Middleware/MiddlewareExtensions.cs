using Microsoft.AspNetCore.Builder;

namespace AspNetCoreSharedComponent.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlogalExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}

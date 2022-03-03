using AspNetCoreSharedComponent.HttpContext;
using AspNetCoreHttp = Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AspNetCoreSharedComponent.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly AspNetCoreHttp.RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(AspNetCoreHttp.RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(AspNetCoreHttp.HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception)
            {
                context.Response.Redirect("/Error/500");
            }
            var responseCode = context.Response.StatusCode;
            if (responseCode >= 400 && !context.Request.IsStatisFileRequest())
            {
                if (responseCode == 405)
                    responseCode = 404;
                context.Response.Redirect($"/Error/{responseCode}");
            }
        }
    }
}

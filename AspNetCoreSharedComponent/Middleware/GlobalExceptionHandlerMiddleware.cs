using AspNetCoreSharedComponent.HttpContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AspNetCoreHttp = Microsoft.AspNetCore.Http;

namespace AspNetCoreSharedComponent.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(AspNetCoreHttp.RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<GlobalExceptionHandlerMiddleware>();
        }

        public async Task Invoke(AspNetCoreHttp.HttpContext context)
        {
            try
            {
                await _next(context);
                var responseCode = context.Response.StatusCode;
                var endpoint = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>();
                _logger.LogInformation($"Controller: {endpoint?.ControllerName}, Action: {endpoint?.ActionName}");
                _logger.LogInformation(
                    $"Request {context.Request.Method} to {context.Request.Path} has returned {responseCode}");
                if (ShouldRedirect(context))
                {
                    if (responseCode == 405)
                        responseCode = 404;
                    context.Response.Redirect($"/Error/{responseCode}");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    $"Request {context.Request.Method} " +
                    $"to {context.Request.Path} has resulted in an error. Message is: {e.Message}");
                _logger.LogError(e.StackTrace);
                if (!context.Request.Path.Value!.StartsWith("/api"))
                {
                    context.Response.Redirect("/Error/500");
                }
            }
        }

        private static bool ShouldRedirect(AspNetCoreHttp.HttpContext httpContext)
        {
            var responseCode = httpContext.Response.StatusCode;
            return responseCode >= 400 && !httpContext.Request.IsStatisFileRequest()
                && !httpContext.Request.Path.Value!.StartsWith("/api");
        }
    }
}

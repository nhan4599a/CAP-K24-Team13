using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using AspNetCoreHttp = Microsoft.AspNetCore.Http;

namespace AspNetCoreSharedComponent.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly AspNetCoreHttp.RequestDelegate _next;

        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(AspNetCoreHttp.RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<GlobalExceptionHandlerMiddleware>();
        }

        public async Task Invoke(AspNetCoreHttp.HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogInformation(
                    $"Request {context.Request.Method} " +
                    $"to {context.Request.Path} has resulted in an error. Message is: {e.Message}");
                context.Response.Redirect("/Error/500");
            }
            var responseCode = context.Response.StatusCode;
            _logger.LogInformation(
                $"Request {context.Request.Method} to {context.Request.Path} has returned {responseCode}");
            if (responseCode >= 400)
            {
                if (responseCode == 405)
                    responseCode = 404;
                context.Response.Redirect($"/Error/{responseCode}");
            }
        }
    }
}

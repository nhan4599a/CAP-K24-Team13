using AspNetCoreSharedComponent.HttpContext;
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

        private readonly bool _includeErrorPath;

        public GlobalExceptionHandlerMiddleware(AspNetCoreHttp.RequestDelegate next,
            ILoggerFactory loggerFactory, bool includeErrorPath = false)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<GlobalExceptionHandlerMiddleware>();
            _includeErrorPath = includeErrorPath;
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
                var queryString = _includeErrorPath
                    ? $"?source={context.Request.Path}"
                    : string.Empty;
                context.Response.Redirect("/Error/500" + queryString);
            }
            var responseCode = context.Response.StatusCode;
            _logger.LogInformation(
                $"Request {context.Request.Method} to {context.Request.Path} has returned {responseCode}");
            if (responseCode >= 400 && !context.Request.IsStatisFileRequest())
            {
                if (responseCode == 405)
                    responseCode = 404;
                context.Response.Redirect($"/Error/{responseCode}");
            }
        }
    }
}

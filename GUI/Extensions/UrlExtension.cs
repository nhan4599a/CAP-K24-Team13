using Microsoft.AspNetCore.Http;
using System.Net;

namespace GUI.Extensions
{
    public static class UrlExtension
    {
        private static string GetCurrentUrl(this HttpRequest request)
        {
            return $"{request.GetProtocol()}://{request.Host}{request.Path}";
        }

        private static string GetProtocol(this HttpRequest request)
        {
            return request.IsHttps ? "https" : "http";
        }

        private static string GetCurrentUrl(this HttpContext httpContext)
        {
            return GetCurrentUrl(httpContext.Request);
        }

        public static string RedirectWithCurrentUrl(this HttpContext httpContext, string destinationUrl, string parameterName)
        {
            return $"{destinationUrl}?{parameterName}={WebUtility.UrlEncode(httpContext.GetCurrentUrl())}";
        }
    }
}

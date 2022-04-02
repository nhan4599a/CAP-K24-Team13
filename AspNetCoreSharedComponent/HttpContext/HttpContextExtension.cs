using Microsoft.AspNetCore.Http;
using System.IO;
namespace AspNetCoreSharedComponent.HttpContext
{
    public static class HttpContextExtension
    {
        public static bool IsStatisFileRequest(this HttpRequest context)
        {
            return !string.IsNullOrEmpty(Path.GetExtension(context.Path));
        }
    }
}

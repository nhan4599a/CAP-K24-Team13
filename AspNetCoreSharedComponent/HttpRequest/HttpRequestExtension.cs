using AspNetCoreHttp = Microsoft.AspNetCore.Http;
namespace AspNetCoreSharedComponent.HttpRequest
{
    public static class HttpRequestExtension
    {
        public static bool IsStatisFileRequest(this AspNetCoreHttp.HttpRequest context)
        {
            return !string.IsNullOrEmpty(Path.GetExtension(context.Path));
        }
    }
}

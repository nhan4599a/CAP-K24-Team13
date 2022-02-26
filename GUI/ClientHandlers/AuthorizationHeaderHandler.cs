using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GUI.ClientHandlers
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public AuthorizationHeaderHandler()
        {
            _logger = LoggerFactory.Create(options =>
            {
                options.AddConsole();
            }).CreateLogger<AuthorizationHeaderHandler>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(request.RequestUri.ToString());
            if (request.Headers.Authorization == null || request.Headers.Authorization.Parameter == null)
                _logger.LogInformation("You do not provide any authorization header");
            else
                _logger.LogInformation(request.Headers.Authorization.Parameter);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}

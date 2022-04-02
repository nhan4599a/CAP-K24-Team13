using SendGrid;
using SendGridHelper = SendGrid.Helpers.Mail;
using Shared.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCoreSharedComponent.Mail
{
    public class MailHelper
    {
        public string ApiKey { get; init; }

        private readonly SendGridClient _client;

        private readonly SendGridHelper.EmailAddress _from;

        private readonly ILogger<MailHelper> _logger;

        public MailHelper(string apiKey, string senderAddress, ILoggerFactory loggerFactory, string? senderName = null)
        {
            ApiKey = apiKey;
            _client = new SendGridClient(ApiKey);
            _from = new SendGridHelper.EmailAddress(senderAddress, senderName);
            _logger = loggerFactory.CreateLogger<MailHelper>();
        }

        public async Task<bool> SendEmail(MailRequest mailRequest)
        {
            var to = new SendGridHelper.EmailAddress(mailRequest.Receiver);

            var mail = mailRequest.IsHtmlMessage
                ? SendGridHelper.MailHelper.CreateSingleEmail(_from, to, mailRequest.Subject, "", mailRequest.Body)
                : SendGridHelper.MailHelper.CreateSingleEmail(_from, to, mailRequest.Subject, mailRequest.Body, "");

            var response = await _client.SendEmailAsync(mail);

            _logger.LogInformation($"Send email completed {response.IsSuccessStatusCode}, Message: {await response.Body.ReadAsStringAsync()}");
            return response.IsSuccessStatusCode;
        }
    }
}

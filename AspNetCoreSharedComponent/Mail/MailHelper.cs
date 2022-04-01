using SendGrid;
using SendGridHelper = SendGrid.Helpers.Mail;
using Shared.Models;
using System.Threading.Tasks;

namespace AspNetCoreSharedComponent.Mail
{
    public class MailHelper
    {
        public string ApiKey { get; init; }

        private readonly SendGridClient _client;

        private readonly SendGridHelper.EmailAddress _from;

        public MailHelper(string apiKey, string senderAddress, string? senderName = null)
        {
            ApiKey = apiKey;
            _client = new SendGridClient(ApiKey);
            _from = new SendGridHelper.EmailAddress(senderAddress, senderName);
        }

        public async Task<bool> SendEmail(MailRequest mailRequest)
        {
            var to = new SendGridHelper.EmailAddress(mailRequest.Receiver);

            var mail = mailRequest.IsHtmlMessage
                ? SendGridHelper.MailHelper.CreateSingleEmail(_from, to, mailRequest.Subject, "", mailRequest.Body)
                : SendGridHelper.MailHelper.CreateSingleEmail(_from, to, mailRequest.Subject, mailRequest.Body, "");

            return (await _client.SendEmailAsync(mail)).IsSuccessStatusCode;
        }
    }
}

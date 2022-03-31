using AspNetCoreSharedComponent.Mail.Events;
using Microsoft.Extensions.Logging;
using Shared.Models;
using System;
using System.Net.Mail;

namespace AspNetCoreSharedComponent.Mail
{
    public class GmailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        private readonly ILogger<GmailService> _logger;

        public event EventHandler<MailSentAsyncEventArgs>? MailSent;

        public GmailService(SmtpClient smtpClient, ILoggerFactory loggerFactory)
        {
            _smtpClient = smtpClient;
            _logger = loggerFactory.CreateLogger<GmailService>();
        }

        public void SendMail(MailRequest mailRequest)
        {
            MailMessage mail = new()
            {
                From = new MailAddress(mailRequest.Sender!)
            };
            mail.To.Add(new MailAddress(mailRequest.Receiver!));
            mail.Subject = mailRequest.Subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = mailRequest.Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = mailRequest.IsHtmlMessage;
            mail.Priority = MailPriority.High;
            foreach (string cc in mailRequest.Cc!)
                mail.CC.Add(new MailAddress(cc));
            foreach (string bcc in mailRequest.Bcc!)
                mail.Bcc.Add(new MailAddress(bcc));
            _smtpClient!.SendCompleted += (sender, args) =>
            {
                OnMailSent(new MailSentAsyncEventArgs(args));
            };
            _smtpClient.SendAsync(mail, mail);
        }

        protected virtual void OnMailSent(MailSentAsyncEventArgs args)
        {
            _logger.LogInformation($"Username: {_smtpClient.Credentials.GetCredential("smtp.gmail.com", 587, "smtp").UserName}");
            _logger.LogInformation($"Password: {_smtpClient.Credentials.GetCredential("smtp.gmail.com", 587, "smtp").Password}");
            args.MailMessage?.Dispose();
            _logger.LogInformation($"Canceled: {args.Cancelled}, Error: {args.Error}");
            _logger.LogInformation($"Canceled: {args.Cancelled}, Error: {args.Error}");
            MailSent?.Invoke(this, args);
        }
    }
}

using AuthServer.Events;
using AuthServer.Models;
using System.Net;
using System.Net.Mail;

namespace AuthServer.Services
{
    public class GmailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        public string MailProvider => "GMAIL";

        public string MailAddress => Username;

        public string Username { get; }

        public string Password { get; }

        public event EventHandler<MailSentAsyncEventArgs>? MailSent;

        public GmailService(SmtpClient smtpClient, IConfiguration configuration)
        {
            var username = configuration.GetValue<string>("GMAIL-USERNAME");
            var password = configuration.GetValue<string>("GMAIL-PASSWORD");
            _smtpClient = smtpClient;
            _smtpClient.Credentials = new NetworkCredential(username, password);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.EnableSsl = true;
            _smtpClient.Host = "smtp.gmail.com";
            _smtpClient.Port = 587;
            Username = username;
            Password = password;
        }

        public void SendMail(MailRequest mailRequest)
        {
            MailMessage mail = new()
            {
                From = new MailAddress(mailRequest.Sender)
            };
            mail.To.Add(new MailAddress(mailRequest.Receiver));
            mail.Subject = mailRequest.Subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = mailRequest.Body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = mailRequest.IsHtmlMessage;
            mail.Priority = MailPriority.High;
            foreach (string cc in mailRequest.Cc)
                mail.CC.Add(new MailAddress(cc));
            foreach (string bcc in mailRequest.Bcc)
                mail.Bcc.Add(new MailAddress(bcc));
            _smtpClient.SendCompleted += (sender, args) =>
            {
                OnMailSent(new MailSentAsyncEventArgs(args));
            };
            _smtpClient.SendAsync(mail, mail);
        }

        protected virtual void OnMailSent(MailSentAsyncEventArgs args)
        {
            args.MailMessage?.Dispose();
            MailSent?.Invoke(this, args);
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

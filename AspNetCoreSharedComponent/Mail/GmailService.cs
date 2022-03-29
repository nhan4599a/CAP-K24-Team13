using AspNetCoreSharedComponent.Mail.Events;
using Shared.Models;
using System;
using System.Net.Mail;

namespace AspNetCoreSharedComponent.Mail
{
    public class GmailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        public event EventHandler<MailSentAsyncEventArgs>? MailSent;

        public GmailService(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
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
            args.MailMessage?.Dispose();
            MailSent?.Invoke(this, args);
        }
    }
}

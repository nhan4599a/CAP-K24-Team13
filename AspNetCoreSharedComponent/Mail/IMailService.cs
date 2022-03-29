using Shared.Models;

namespace AspNetCoreSharedComponent.Mail
{
    public interface IMailService
    {
        void SendMail(MailRequest mailRequest);
    }
}

using AuthServer.Models;

namespace AuthServer
{
    public interface IMailService
    {
        void SendMail(MailRequest mailRequest);
    }
}

using AuthServer.Models;

namespace AuthServer
{
    public interface IMailService : IDisposable
    {
        string MailProvider { get; }

        string MailAddress { get; }

        void SendMail(MailRequest mailRequest);
    }
}

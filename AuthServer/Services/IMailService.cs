using AuthServer.Models;

namespace AuthServer
{
    public interface IMailService : IDisposable
    {
        string MailProvider { get; }

        void SendMail(MailRequest mailRequest);
    }
}

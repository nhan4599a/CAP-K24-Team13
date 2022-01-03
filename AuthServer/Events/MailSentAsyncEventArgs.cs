using System.ComponentModel;
using System.Net.Mail;

namespace AuthServer.Events
{
    public class MailSentAsyncEventArgs : AsyncCompletedEventArgs
    {
        public MailMessage? MailMessage { get; }

        public MailSentAsyncEventArgs(AsyncCompletedEventArgs args) : base(args.Error, args.Cancelled, args.UserState)
        {
            if (args.UserState != null)
                MailMessage = args.UserState as MailMessage;
        }
    }
}

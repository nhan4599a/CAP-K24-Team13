namespace AuthServer.Models
{
    public class MailRequest
    {
        public string? Sender { get; set; }

        public string? Receiver { get; set; }

        public string[]? Cc { get; set; }

        public string[]? Bcc { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }

        public bool IsHtmlMessage { get; set; }
    }
}

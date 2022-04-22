namespace Shared.RequestModels
{
    public class RatingRequestModel
    {
        public int InvoiceId { get; set; }

        public string ProductId { get; set; }

        public string Message { get; set; }

        public int Star { get; set; }
    }
}

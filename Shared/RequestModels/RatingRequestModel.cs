namespace Shared.RequestModels
{
    public class RatingRequestModel
    {
        public string ProductId { get; set; }

        public string UserId { get; set; }

        public string Message { get; set; }

        public int Star { get; set; }
    }
}

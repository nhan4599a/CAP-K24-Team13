namespace Shared.RequestModels
{
    public class AddOrEditQuantityCartItemRequestModel
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public string UserId { get; set; }
    }
}

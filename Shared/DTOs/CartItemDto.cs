namespace Shared.DTOs
{
    public class CartItemDTO
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public int Discount { get; set; }

        public int Quantity { get; set; }

        public string Image { get; set; }

        public bool IsAvailable { get; set; }
    }
}
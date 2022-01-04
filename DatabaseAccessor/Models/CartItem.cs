namespace Shared.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ShopName { get; set; }
    }
}

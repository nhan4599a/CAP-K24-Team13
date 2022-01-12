using System.Collections.Generic;

namespace Shared.DTOs
{
    public class CartDTO
    {
        public int CartItemId { get; set; }

        public string UserId { get; set; }

        public string ShopName { get; set; }

        public List<CartItemDto> Items { get; set; }
    }
}
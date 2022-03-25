using System.Text.Json.Serialization;

namespace Shared.DTOs
{
    public class ShopDTO
    {
        public int Id { get; set; }

        [JsonPropertyName("shop_Name")]
        public string ShopName { get; set; }

        public string Description { get; set; }

        public string Avatar { get; set; }

        public string Floor { get; set; }

        public string Position { get; set; }
    }
}

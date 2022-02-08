using System.Collections.Generic;

namespace Shared.DTOs
{
    public class ProductDTO : MinimalProductDTO
    {
        public string Description { get; set; }

        public int Quantity { get; set; }

        public int Discount { get; set; }

        public string CategoryName { get; set; }

        public string[] Images { get; set; }

        public bool IsDisabled { get; set; }

        public  List<RatingDTO> Comments { get; set; }
    }
}
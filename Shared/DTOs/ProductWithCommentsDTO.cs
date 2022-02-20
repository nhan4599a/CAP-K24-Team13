using System.Collections.Generic;

namespace Shared.DTOs
{
    public class ProductWithCommentsDTO : ProductDTO
    {
        public IList<RatingDTO> Comments { get; set; }
    }
}

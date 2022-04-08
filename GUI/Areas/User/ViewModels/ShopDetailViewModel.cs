using Shared.DTOs;
using System.Collections.Generic;

namespace GUI.Areas.User.ViewModels
{
    public class ShopDetailViewModel
    {
        public Dictionary<int, List<ProductDTO>> Products { get; set; }

        public List<CategoryDTO> Categories { get; set; }

        public List<MinimalProductDTO> BestSeller { get; set; }

        public ShopDTO Shop { get; set; }
    }
}

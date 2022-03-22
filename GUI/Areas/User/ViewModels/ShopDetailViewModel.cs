using Shared.DTOs;
using Shared.Models;

namespace GUI.Areas.User.ViewModels
{
    public class ShopDetailViewModel
    {
        public PaginatedList<ProductDTO> Products { get; set; }

        public PaginatedList<CategoryDTO> Categories { get; set; }

        public ShopDTO Shop { get; set; }
    }
}

using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;

namespace GUI.Areas.User.ViewModels
{
    public class HomeCategoryViewModel
    {
        public List<CategoryDTO> Categories { get; set; }

        public PaginatedList<ProductDTO> Products { get; set; }
    }
}

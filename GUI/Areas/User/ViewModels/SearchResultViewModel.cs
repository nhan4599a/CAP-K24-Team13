using Shared.DTOs;
using Shared.Models;

namespace GUI.Areas.User.ViewModels
{
	public class SearchResultViewModel
	{
		public PaginatedList<ProductDTO> Products { get; set; }

		public PaginatedList<ShopDTO> Shops { get; set; }
	}
}

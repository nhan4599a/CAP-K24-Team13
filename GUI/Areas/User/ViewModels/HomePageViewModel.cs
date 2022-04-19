using Shared.DTOs;
using System.Collections.Generic;

namespace GUI.Areas.User.ViewModels
{
    public class HomePageViewModel
    {
        public List<(int Id, string ShopName)> Shops { get; set; }

        public List<MinimalProductDTO> BestSellerProducts { get; set; }

        public List<MinimalProductDTO> TopMostSaleOffProducts { get; set; }

        public Dictionary<string, List<ProductDTO>> Products { get; set; }
    }
}

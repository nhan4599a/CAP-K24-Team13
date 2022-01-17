using DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopProductService.Controllers
{
    [ApiController]
    [Route("/api/shop")]
    public class ShopController : ControllerBase
    {
        private List<ShopDTO> FakeShops = new()
        {
            new ShopDTO { Id = 1, Name = "Adidas", Image = "https://gigamall.com.vn/data/2019/09/05/15055504_LOGO-REEBOK-500x500.jpg" },
            new ShopDTO { Id = 2, Name = "Future World", Image = "https://gigamall.com.vn/data/2019/09/05/15055504_LOGO-REEBOK-500x500.jpg" },
            new ShopDTO { Id = 3, Name = "Apple Store", Image = "https://gigamall.com.vn/data/2019/09/05/15055504_LOGO-REEBOK-500x500.jpg" },
            new ShopDTO { Id = 4, Name = "Nike", Image = "https://gigamall.com.vn/data/2019/09/05/15055504_LOGO-REEBOK-500x500.jpg" },
            new ShopDTO { Id = 5, Name = "Bitis", Image = "https://gigamall.com.vn/data/2019/09/05/15055504_LOGO-REEBOK-500x500.jpg" },
            new ShopDTO { Id = 6, Name = "Converse", Image = "https://gigamall.com.vn/data/2019/09/05/15055504_LOGO-REEBOK-500x500.jpg" }
        };

        [HttpGet("search")]
        public PaginatedList<ShopDTO> FindShops(string keyword, int pageNumber, int? pageSize)
        {
            return FakeShops.Where(shop => shop.Name.Contains(keyword)).Paginate(pageNumber, pageSize);
        }
    }
}

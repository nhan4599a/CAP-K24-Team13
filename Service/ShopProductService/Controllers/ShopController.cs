using DatabaseAccessor;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Shared.Models;
using Shared.RequestModels;
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
            new ShopDTO { Id = 0, Name = "Adidas", Image = "data:image/png;base64,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" },
            new ShopDTO { Id = 1, Name = "Future World", Image = "https://futureworld.com.vn/media/logo/stores/1/FW_Black_logo_1.png" },
            new ShopDTO { Id = 2, Name = "Apple Store", Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Apple_logo_black.svg/488px-Apple_logo_black.svg.png" },
            new ShopDTO { Id = 3, Name = "Nike", Image = "https://logos-world.net/wp-content/uploads/2020/04/Nike-Logo-1978-present.jpg" },
            new ShopDTO { Id = 4, Name = "Bitis", Image = "https://marathonhcmc.com/wp-content/uploads/2021/02/bitis-1-1.png" },
            new ShopDTO { Id = 5, Name = "Converse", Image = "https://drake.vn/image/catalog/H%C3%ACnh%20content/converse-chuck-taylor-all-star-logo-play/Converse-chuck-taylor-all-star-logo-play-7.jpg" }
        };

        [HttpGet("search")]
        public PaginatedList<ShopDTO> FindShops([FromQuery] SearchRequestModel requestModel)
        {
            var result = FakeShops.Where(shop => shop.Name.ToLower().Contains(requestModel.Keyword.ToLower()))
                .Paginate(requestModel.PaginationInfo.PageNumber, requestModel.PaginationInfo.PageSize);
            return result;
        }
    }
}

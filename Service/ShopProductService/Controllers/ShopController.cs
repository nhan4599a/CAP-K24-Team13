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
        private readonly List<ShopDTO> FakeShops = new()
        {
            new ShopDTO { Id = 0, Name = "data:image/jpeg;base64,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" },
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

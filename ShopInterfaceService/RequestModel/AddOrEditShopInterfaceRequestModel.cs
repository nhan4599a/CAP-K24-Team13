using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ShopInterfaceService.RequestModel
{
    public class AddOrEditShopInterfaceRequestModel
    {
        public string ShopAddress { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public List<IFormFile> Images { get; set; }
    }
}

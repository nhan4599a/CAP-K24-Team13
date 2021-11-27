using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProductService.RequestModel
{
    public class AddCategoryRequestModel
    {
        public string categoryName { get; set; }

        public int special { get; set; }
    }
}

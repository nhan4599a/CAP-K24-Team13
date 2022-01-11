using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels
{
    public class AddOrEditQuantityCartItemRequestModel
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ShopName { get; set; }
        public string UserId { get; set; }
    }
}

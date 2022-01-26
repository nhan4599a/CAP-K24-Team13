using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.RequestModels
{
    public class CheckOutRequestModel
    {
        public string UserId { get; set; }

        public List<string> ProductIds { get; set; }

        public string ShippingAddress { get; set; }
    }
}

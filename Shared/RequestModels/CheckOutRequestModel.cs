using System.Collections.Generic;

namespace Shared.RequestModels
{
    public class CheckOutRequestModel
    {
        public string UserId { get; set; }

        public List<string> ProductIds { get; set; }

        public string ShippingName { get; set; }

        public string ShippingPhone { get; set; }

        public string ShippingAddress { get; set; }

        public string OrderNotes { get; set; }
    }
}

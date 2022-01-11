using System.Collections.Generic;

namespace GUI.Areas.Customer.Models
{
    public class ShopViewModel
    {
        public int Id { get; set; }

        public int ShopId { get; set; }

        public string ShopAddress { get; set; }

        public string ShopEmail { get; set; }

        public string ShopName { get; set; }

        public string ShopPhoneNumber { get; set; }

        public string ShopDescription { get; set; }

        public List<string> Images { get; set; }
    }
}

using System.Collections.Generic;

namespace Shared.DTOs
{
    public class InvoiceDetailDTO : InvoiceDTO
    {
        public int ShopId { get; set; }

        public string ShippingAddress { get; set; }

        public List<OrderItemDTO> Products { get; set; }
    }
}

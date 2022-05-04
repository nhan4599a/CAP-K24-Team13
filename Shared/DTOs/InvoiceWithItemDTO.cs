using System.Collections.Generic;

namespace Shared.DTOs
{
    public class InvoiceWithItemDTO : InvoiceDTO
    {
        public List<InvoiceItemDTO> Products { get; set; }
    }
}

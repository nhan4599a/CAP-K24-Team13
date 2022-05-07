using System.Collections.Generic;

namespace Shared.DTOs
{
    public class FullInvoiceDTO : InvoiceWithItemDTO
    {
        public List<InvoiceStatusChangedHistoryDTO> StatusHistories { get; set; }
    }
}
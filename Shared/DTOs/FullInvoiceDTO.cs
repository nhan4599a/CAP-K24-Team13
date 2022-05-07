using System.Collections.Generic;

namespace Shared.DTOs
{
    public class FullInvoiceDTO : InvoiceWithItemDTO
    {
        public bool IsReported { get; set; }

        public List<InvoiceStatusChangedHistoryDTO> StatusHistories { get; set; }
    }
}

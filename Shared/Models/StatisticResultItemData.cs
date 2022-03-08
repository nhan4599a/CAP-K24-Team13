using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticResultItemData
    {
        public int Total { get; set; }

        [JsonPropertyName("newOrders")]
        public int NewInvoiceCount { get; set; }

        [JsonPropertyName("confirmedOrders")]
        public int ConfirmedInvoiceCount { get; set; }

        [JsonPropertyName("canceledOrders")]
        public int CanceledInvoiceCount { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticResultItemData
    {
        public int Total { get; set; }

        [JsonPropertyName("newOrders")]
        public int NewInvoiceCount { get; set; }

        [JsonPropertyName("succeedOrders")]
        public int SucceedInvoiceCount { get; set; }

        [JsonPropertyName("canceledOrders")]
        public int CanceledInvoiceCount { get; set; }
    }
}

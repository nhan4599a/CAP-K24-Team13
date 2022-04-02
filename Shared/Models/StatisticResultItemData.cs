using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticResultItemData
    {
        [JsonPropertyName("new")]
        public int NewInvoiceCount { get; set; }

        [JsonPropertyName("succeed")]
        public int SucceedInvoiceCount { get; set; }

        [JsonPropertyName("canceled")]
        public int CanceledInvoiceCount { get; set; }
    }
}

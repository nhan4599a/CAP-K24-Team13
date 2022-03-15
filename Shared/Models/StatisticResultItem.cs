using System.Text.Json.Serialization;

namespace Shared.Models
{
    public class StatisticResultItem
    {
        public StatisticResultItemData Data { get; set; }

        public double Income { get; set; }

        [JsonPropertyName("user")]
        public int UsersCount { get; set; }
    }
}

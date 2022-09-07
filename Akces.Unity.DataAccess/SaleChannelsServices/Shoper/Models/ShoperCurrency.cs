using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperCurrency
    {
        [JsonPropertyName("currency_id")]
        public string CurrencyId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rate")]
        public string Rate { get; set; }

        [JsonPropertyName("order")]
        public string Order { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }

        [JsonPropertyName("rate_sync")]
        public string RateSync { get; set; }

        [JsonPropertyName("rate_date")]
        public string RateDate { get; set; }

        [JsonPropertyName("default")]
        public string Default { get; set; }
    }
}

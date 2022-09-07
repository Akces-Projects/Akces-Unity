using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperPaymentMethod
    {
        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("order")]
        public string Order { get; set; }

        [JsonPropertyName("install")]
        public string Install { get; set; }

        [JsonPropertyName("visible")]
        public string Visible { get; set; }

        [JsonPropertyName("translations")]
        public ShoperTranslations Translations { get; set; }

        [JsonPropertyName("currencies")]
        public List<string> Currencies { get; set; }
    }
}

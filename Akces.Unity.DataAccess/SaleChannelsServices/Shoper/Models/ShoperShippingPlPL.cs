using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperShippingPlPL
    {
        [JsonPropertyName("translation_id")]
        public string TranslationId { get; set; }

        [JsonPropertyName("shipping_id")]
        public string ShippingId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("vendor_description")]
        public string VendorDescription { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }

        [JsonPropertyName("is_default")]
        public string IsDefault { get; set; }

        [JsonPropertyName("lang_id")]
        public string LangId { get; set; }
    }
}

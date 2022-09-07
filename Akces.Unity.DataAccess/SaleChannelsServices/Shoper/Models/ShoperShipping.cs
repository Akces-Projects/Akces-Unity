using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperShipping
    {
        [JsonPropertyName("shipping_id")]
        public string ShippingId { get; set; }

        [JsonPropertyName("cost")]
        public string Cost { get; set; }

        [JsonPropertyName("depend_on_w")]
        public string DependOnW { get; set; }

        [JsonPropertyName("tax_id")]
        public string TaxId { get; set; }

        [JsonPropertyName("max_weight")]
        public string MaxWeight { get; set; }

        [JsonPropertyName("min_weight")]
        public string MinWeight { get; set; }

        [JsonPropertyName("free_shipping")]
        public string FreeShipping { get; set; }

        [JsonPropertyName("order")]
        public string Order { get; set; }

        [JsonPropertyName("pkwiu")]
        public string Pkwiu { get; set; }

        [JsonPropertyName("max_cost")]
        public string MaxCost { get; set; }

        [JsonPropertyName("visible")]
        public string Visible { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("engine")]
        public string Engine { get; set; }

        [JsonPropertyName("zone_id")]
        public string ZoneId { get; set; }

        [JsonPropertyName("callback_url")]
        public string CallbackUrl { get; set; }

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

        [JsonPropertyName("countries")]
        public ShoperCountries Countries { get; set; }

        [JsonPropertyName("ranges")]
        public List<object> Ranges { get; set; }

        [JsonPropertyName("payments")]
        public List<ShoperShippingPayment> Payments { get; set; }

        [JsonPropertyName("gauges")]
        public List<object> Gauges { get; set; }
    }


}

using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Models
{
    public class ShoperPlPL
    {
        [JsonPropertyName("translation_id")]
        public string TranslationId { get; set; }

        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("vendor_description")]
        public string VendorDescription { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }

        [JsonPropertyName("notify")]
        public string Notify { get; set; }

        [JsonPropertyName("notify_mail")]
        public string NotifyMail { get; set; }

        [JsonPropertyName("lang_id")]
        public string LangId { get; set; }
    }
}

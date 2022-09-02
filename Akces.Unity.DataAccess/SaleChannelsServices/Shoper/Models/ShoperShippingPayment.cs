using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Models
{
    public class ShoperShippingPayment
    {
        [JsonPropertyName("cost")]
        public int Cost { get; set; }

        [JsonPropertyName("percent")]
        public bool Percent { get; set; }

        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; }
    }
}

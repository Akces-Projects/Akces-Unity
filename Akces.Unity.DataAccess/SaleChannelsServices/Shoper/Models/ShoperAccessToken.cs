using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperAccessToken
    {
        [JsonPropertyName("access_token")]
        public string Value { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}

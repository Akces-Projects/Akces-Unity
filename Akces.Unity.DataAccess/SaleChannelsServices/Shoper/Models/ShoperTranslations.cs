using System.Text.Json.Serialization;

namespace Unity.SaleChannels.Shoper.Models
{
    public class ShoperTranslations
    {
        [JsonPropertyName("pl_PL")]
        public ShoperTranslation PL { get; set; }

        [JsonPropertyName("de_DE")]
        public ShoperTranslation DE { get; set; }
    }
}

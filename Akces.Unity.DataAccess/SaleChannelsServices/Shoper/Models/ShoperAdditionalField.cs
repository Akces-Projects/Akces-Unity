using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Models
{
    public class ShoperAdditionalField
    {
        [JsonPropertyName("field_id")]
        public string FieldId { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("locate")]
        public string Locate { get; set; }

        [JsonPropertyName("req")]
        public string Req { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }

        [JsonPropertyName("order")]
        public string Order { get; set; }

        [JsonPropertyName("field_value")]
        public string FieldValue { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

}

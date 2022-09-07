using System.Collections.Generic;
using System.Text.Json.Serialization;
using Unity.SaleChannels.Shoper.Models;

namespace Unity.SaleChannels.Shoper.Responses
{
    public class GetShippingsResponse
    {
        [JsonPropertyName("count")]
        public string Count { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("list")]
        public List<ShoperShipping> Shippings { get; set; }
    }
}

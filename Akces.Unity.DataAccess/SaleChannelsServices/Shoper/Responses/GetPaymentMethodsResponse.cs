using Akces.Unity.DataAccess.Services.Shoper.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Responses
{
    public class GetPaymentMethodsResponse
    {
        [JsonPropertyName("count")]
        public string Count { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("list")]
        public List<ShoperPaymentMethod> PaymentMethods { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Models
{
    public class ShoperTranslations
    {
        [JsonPropertyName("pl_PL")]
        public ShoperPlPL PlPL { get; set; }
    }
}

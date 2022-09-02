using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Models
{
    public class ShoperProduct
    {
        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("producer_id")]
        public string ProducerId { get; set; }

        [JsonPropertyName("group_id")]
        public string GroupId { get; set; }

        [JsonPropertyName("tax_id")]
        public string TaxId { get; set; }

        [JsonPropertyName("add_date")]
        public string AddDate { get; set; }

        [JsonPropertyName("edit_date")]
        public string EditDate { get; set; }

        [JsonPropertyName("other_price")]
        public string OtherPrice { get; set; }

        [JsonPropertyName("pkwiu")]
        public string Pkwiu { get; set; }

        [JsonPropertyName("unit_id")]
        public string UnitId { get; set; }

        [JsonPropertyName("in_loyalty")]
        public string InLoyalty { get; set; }

        [JsonPropertyName("loyalty_score")]
        public object LoyaltyScore { get; set; }

        [JsonPropertyName("loyalty_price")]
        public object LoyaltyPrice { get; set; }

        [JsonPropertyName("bestseller")]
        public string Bestseller { get; set; }

        [JsonPropertyName("newproduct")]
        public string Newproduct { get; set; }

        [JsonPropertyName("dimension_w")]
        public string DimensionW { get; set; }

        [JsonPropertyName("dimension_h")]
        public string DimensionH { get; set; }

        [JsonPropertyName("dimension_l")]
        public string DimensionL { get; set; }

        [JsonPropertyName("vol_weight")]
        public string VolWeight { get; set; }

        [JsonPropertyName("currency_id")]
        public object CurrencyId { get; set; }

        [JsonPropertyName("gauge_id")]
        public object GaugeId { get; set; }

        [JsonPropertyName("unit_price_calculation")]
        public string UnitPriceCalculation { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("category_id")]
        public string CategoryId { get; set; }

        [JsonPropertyName("promo_price")]
        public object PromoPrice { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("ean")]
        public string Ean { get; set; }

        [JsonPropertyName("is_product_of_day")]
        public bool IsProductOfDay { get; set; }
    }
}

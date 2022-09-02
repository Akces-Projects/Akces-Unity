using Akces.Unity.Models.SaleChannels;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Akces.Unity.DataAccess.Services.Shoper.Models
{
    public class ShoperOrderPosition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("stock_id")]
        public string StockId { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("discount_perc")]
        public string DiscountPerc { get; set; }

        [JsonPropertyName("quantity")]
        public string Quantity { get; set; }

        [JsonPropertyName("delivery_time")]
        public string DeliveryTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("pkwiu")]
        public string Pkwiu { get; set; }

        [JsonPropertyName("tax")]
        public string Tax { get; set; }

        [JsonPropertyName("tax_value")]
        public string TaxValue { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; }

        [JsonPropertyName("option")]
        public string Option { get; set; }

        [JsonPropertyName("unit_fp")]
        public string UnitFp { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("loyalty")]
        public object Loyalty { get; set; }

        [JsonPropertyName("delivery_time_hours")]
        public string DeliveryTimeHours { get; set; }

        [JsonPropertyName("text_options")]
        public List<object> TextOptions { get; set; }

        [JsonPropertyName("file_options")]
        public List<object> FileOptions { get; set; }
        public ShoperProduct Product { get; internal set; }

        internal Product ToProduct()
        {
            var product = new Product();
            //{
            product.Symbol = this.Code;
            product.Name = this.Name;
            product.Id = this.ProductId;
            product.Price = decimal.Parse(this.Price, CultureInfo.InvariantCulture);
            product.Quantity = decimal.Parse(this.Quantity, CultureInfo.InvariantCulture);
            product.DiscountPercentage = decimal.Parse(this.DiscountPerc, CultureInfo.InvariantCulture);
            product.EAN = this.Product?.Ean;
            product.CN = "";
            product.Tax = this.TaxValue;
            product.Unit = this.Unit;
            //};

            return product;
        }
    }
}

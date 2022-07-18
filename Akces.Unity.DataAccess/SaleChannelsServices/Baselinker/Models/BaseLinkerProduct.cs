using Akces.Unity.Models.SaleChannels;
using System.Collections.Generic;
using System.Linq;

namespace Akces.Unity.Models.SaleChannels.Baselinker
{
    public class BaseLinkerProduct
    {
        #region Properties
        public string storage { get; set; }
        public int storage_id { get; set; }
        public string order_product_id { get; set; }
        public string product_id { get; set; }
        public string variant_id { get; set; }
        public string name { get; set; }
        public string attributes { get; set; }
        public string sku { get; set; }
        public string ean { get; set; }
        public string location { get; set; }
        public string auction_id { get; set; }
        public decimal price_brutto { get; set; }
        public int tax_rate { get; set; }
        public int quantity { get; set; }
        public decimal weight { get; set; }
        public int bundle_id { get; set; }
        #endregion

        public Product ToProduct()
        {
            var attributes = 
                string.IsNullOrWhiteSpace(this.attributes) ? new Dictionary<string, object>() : 
                this.attributes.Split('|')
                .Select(x => new { Key = x.Split(':')[0].Trim(), Value = (object)x.Split(':')[1].Trim() })
                .ToDictionary(a => a.Key, a => a.Value);

            var product = new Product()
            {
                Name = name,
                Price = price_brutto,
                Quantity = quantity,
                Tax = tax_rate.ToString(),
                EAN = ean,
                Symbol = sku,
                Attributes = attributes
            };

            return product; 
        }
    }   
}

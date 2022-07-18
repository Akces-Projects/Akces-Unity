
using System.Collections.Generic;

namespace Akces.Unity.Models.SaleChannels
{
    public class Product
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string EAN { get; set; }
        public string CN { get; set; }
        public decimal Quantity { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Price { get; set; }
        public string Tax { get; set; }
        public string Currency { get; set; }
        public string Unit { get; set; }
        public decimal DiscountPercentage { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}

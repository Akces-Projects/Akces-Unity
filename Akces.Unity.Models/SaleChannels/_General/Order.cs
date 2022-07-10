using System;
using System.Collections.Generic;

namespace Akces.Unity.Models.SaleChannels
{
    public class Order
    {
        public string SourceSaleChannelName { get; set; }
        public string Original { get; set; }
        public Contractor Purchaser { get; set; }
        public DateTime OriginalDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string OriginalPlace { get; set; }
        public string Branch { get; set; }
        public string Warehouse { get; set; }
        public string Title { get; set; }
        public string Annotation { get; set; }
        public string Currency { get; set; }
        public List<Product> Products { get; set; }
        public List<Payment> Payments { get; set; }
        public Delivery Delivery { get; set; }
        public string Subtitle { get; set; }
        public bool Confirmed { get; set; }
    }
}


namespace Akces.Unity.Models.SaleChannels
{
    public class Delivery
    {
        public DeliveryAddress DeliveryAddress { get; set; }
        public decimal DeliveryCost { get; set; }
        public string DeliveryTax { get; set; }
        public string DeliveryMethod { get; set; }
        public string PackageNumber { get; set; }
    }
}

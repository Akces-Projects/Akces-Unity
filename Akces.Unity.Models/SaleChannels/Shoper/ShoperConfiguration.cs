
namespace Akces.Unity.Models.SaleChannels
{
    public class ShoperConfiguration 
    {
        public int Id { get; set; }
        public string BaseAddress { get; set; } = "https://*.shoparena.pl/";
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public int ImportOffset_Hours { get; set; }
        public bool ImportOrders_OnlyConfirmed { get; set; }
        public bool ImportOrders_OnlyCashOnDeliveryOrPaid { get; set; }
    }
}

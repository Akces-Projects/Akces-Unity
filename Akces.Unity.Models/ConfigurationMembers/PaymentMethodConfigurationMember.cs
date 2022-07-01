
namespace Akces.Unity.Models.ConfigurationMembers
{ 
    public class PaymentMethodConfigurationMember
    {
        public int Id { get; set; }
        public string CountriesCodes { get; set; }
        public string ChannelPaymentMethod { get; set; }
        public string ChannelDeliveryMethod { get; set; }
        public string ErpPaymentMethod { get; set; }
        public string CurrencyCode { get; set; }
        public bool Default { get; set; }
    }
}

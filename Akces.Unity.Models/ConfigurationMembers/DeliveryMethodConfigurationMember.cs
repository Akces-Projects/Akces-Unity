
namespace Akces.Unity.Models.ConfigurationMembers
{
    public class DeliveryMethodConfigurationMember 
    {
        public int Id { get; set; }
        public string CountriesCodes { get; set; }
        public string ProductSymbol { get; set; }
        public string ChannelDeliveryMethod { get; set; }
        public string ErpDeliveryMethodName { get; set; }
        public bool Default { get; set; }
    }
}

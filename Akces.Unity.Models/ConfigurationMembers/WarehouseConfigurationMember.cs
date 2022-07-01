
namespace Akces.Unity.Models.ConfigurationMembers
{
    public class WarehouseConfigurationMember
    {
        public int Id { get; set; }
        public string ExternalOrderTemplate { get; set; }
        public string CountriesCodes { get; set; }
        public string ChannelWarehouse { get; set; }
        public string ErpWarehouseSymbol { get; set; }
        public string ErpDocumentStatus { get; set; }
        public bool Default { get; set; }
    }
}

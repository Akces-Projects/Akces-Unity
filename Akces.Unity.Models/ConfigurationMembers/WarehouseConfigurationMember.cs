
namespace Akces.Unity.Models.ConfigurationMembers
{
    public class WarehouseConfigurationMember
    {
        public int Id { get; set; }
        public string CountriesCodes { get; set; }
        public string ChannelWarehouse { get; set; }
        public string NexoWarehouseSymbol { get; set; }
        public string NexoDocumentStatus { get; set; }
        public bool Default { get; set; }
    }
}

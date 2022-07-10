
namespace Akces.Unity.Models.ConfigurationMembers
{
    public class TaxRateConfigurationMember
    {
        public int Id { get; set; }
        public string CountriesCodes { get; set; }
        public string ChannelTaxRate { get; set; }
        public string NexoTaxRateSymbol { get; set; }
        public bool Default { get; set; }
    }
}

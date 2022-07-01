using System.Collections.Generic;
using Akces.Unity.Models.ConfigurationMembers;

namespace Akces.Unity.Models
{
    public class NexoConfiguration
    {
        public int Id { get; set; }
        public virtual List<DeliveryMethodConfigurationMember> DeliveryMethods { get; set; }
        public virtual List<PaymentMethodConfigurationMember> PaymentMethods { get; set; }
        public virtual List<TransactionConfigurationMember> Transactions { get; set; }
        public virtual List<WarehouseConfigurationMember> Warehouses { get; set; }
        public virtual List<TaxRateConfigurationMember> TaxRates { get; set; }
        public virtual List<BranchConfigurationMember> Branches { get; set; }
        public virtual List<UnitConfigurationMember> Units { get; set; }

        public NexoConfiguration()
        {
            DeliveryMethods = new List<DeliveryMethodConfigurationMember>();
            PaymentMethods = new List<PaymentMethodConfigurationMember>();
            Transactions = new List<TransactionConfigurationMember>();
            Warehouses = new List<WarehouseConfigurationMember>();
            TaxRates = new List<TaxRateConfigurationMember>();
            Branches = new List<BranchConfigurationMember>();
            Units = new List<UnitConfigurationMember>();
        }
    }
}

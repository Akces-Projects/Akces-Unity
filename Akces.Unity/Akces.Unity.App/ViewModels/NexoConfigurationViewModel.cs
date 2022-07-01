using Akces.Core.Nexo;
using Akces.Unity.Core;
using Akces.Unity.Core.NexoData;
using Akces.Unity.Core.NexoData.ConfigurationMembers;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{
    public class NexoConfigurationViewModel : ControlViewModel 
    {
        protected readonly UnityService unityService;
        public NexoConfiguration Configuration { get; set; }

        public ObservableCollection<WarehouseConfigurationMember> WarehouseConfigurationMembers { get; set; }
        public ObservableCollection<DeliveryMethodConfigurationMember> DeliveryMethodConfigurationMembers { get; set; }
        public ObservableCollection<PaymentMethodConfigurationMember> PaymentMethodConfigurationMembers { get; set; }
        public ObservableCollection<TransactionConfigurationMember> TransactionConfigurationMembers { get; set; }
        public ObservableCollection<BranchConfigurationMember> BranchConfigurationMembers { get; set; }
        public ObservableCollection<TaxRateConfigurationMember> TaxRateConfigurationMembers { get; set; }
        public ObservableCollection<UnitConfigurationMember> UnitConfigurationMembers { get; set; }
        public ObservableCollection<string> Branches { get; set; }
        public ObservableCollection<string> Warehouses { get; set; }
        public ObservableCollection<string> DeliveryMethods { get; set; }
        public ObservableCollection<string> PaymentMethods { get; set; }
        public ObservableCollection<string> Transactions { get; set; }
        public ObservableCollection<string> Units { get; set; }
        public ObservableCollection<string> TaxRates { get; set; }
        public ObservableCollection<string> Statuses { get; set; }

        public ICommand SaveConfigurationCommand { get; set; }

        public NexoConfigurationViewModel(HostViewModel host) : base(host)
        {
            unityService = ServicesProvider.GetService<UnityService>(unityService);
            SaveConfigurationCommand = CreateCommand(SaveConfiguration, (err) => Host.ShowError(err));
        }

        public void LoadConfigurationMembers()
        {
            DeliveryMethodConfigurationMembers = new ObservableCollection<DeliveryMethodConfigurationMember>(Configuration.DeliveryMethods);
            PaymentMethodConfigurationMembers = new ObservableCollection<PaymentMethodConfigurationMember>(Configuration.PaymentMethods);
            TransactionConfigurationMembers = new ObservableCollection<TransactionConfigurationMember>(Configuration.Transactions);
            WarehouseConfigurationMembers = new ObservableCollection<WarehouseConfigurationMember>(Configuration.Warehouses);
            TaxRateConfigurationMembers = new ObservableCollection<TaxRateConfigurationMember>(Configuration.TaxRates);
            BranchConfigurationMembers = new ObservableCollection<BranchConfigurationMember>(Configuration.Branches);
            UnitConfigurationMembers = new ObservableCollection<UnitConfigurationMember>(Configuration.Units);
        }
        public void LoadNexoOptions()
        {
            var nexoConfigurationsManager = ServicesProvider
                .GetService<NexoContext>()
                .GetManager<NexoSettingsManager>();

            var warehouses = nexoConfigurationsManager.GetWarehouses().Select(x => x.Symbol).ToList();
            var deliveryMethods = nexoConfigurationsManager.GetDeliveryMethods().Select(x => x.Name).ToList();
            var paymentMethods = nexoConfigurationsManager.GetPaymentMethods().Select(x => x.Name).ToList();
            var transactions = nexoConfigurationsManager.GetTransactions().Select(x => x.Symbol).ToList();
            var branches = nexoConfigurationsManager.GetBranches().Select(x => x.Symbol).ToList();
            var units = nexoConfigurationsManager.GetUnits().Select(x => x.Symbol).ToList();
            var taxRates = nexoConfigurationsManager.GetTaxRates().Select(x => x.Symbol).ToList();

            DeliveryMethods = new ObservableCollection<string>(deliveryMethods);
            PaymentMethods = new ObservableCollection<string>(paymentMethods);
            Transactions = new ObservableCollection<string>(transactions);
            Warehouses = new ObservableCollection<string>(warehouses);
            TaxRates = new ObservableCollection<string>(taxRates);
            Branches = new ObservableCollection<string>(branches);
            Units = new ObservableCollection<string>(units);
            Statuses = new ObservableCollection<string>() { "B", "C", "R" };
        }

        public void SaveConfiguration()
        {
            Configuration.Transactions = TransactionConfigurationMembers.ToList();
            Configuration.DeliveryMethods = DeliveryMethodConfigurationMembers.ToList();
            Configuration.PaymentMethods = PaymentMethodConfigurationMembers.ToList();
            Configuration.Warehouses = WarehouseConfigurationMembers.ToList();
            Configuration.Branches = BranchConfigurationMembers.ToList();
            Configuration.Units = UnitConfigurationMembers.ToList();
            Configuration.TaxRates = TaxRateConfigurationMembers.ToList();

            unityService.SaleChannelAccountsManager.SaveNexoConfiguration(Configuration);
            Host.Window.Close();
        }
    }
}

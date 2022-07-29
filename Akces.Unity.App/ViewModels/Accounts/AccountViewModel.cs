using System;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Akces.Core.Nexo;
using Akces.Wpf.Models;
using Akces.Wpf.Helpers;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.Models.ConfigurationMembers;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.NexoManagers;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.ViewModels
{
    internal abstract class AccountViewModel<T> : ControlViewModel where T : Account, new()
    {
        private ObservableCollection<WarehouseConfigurationMember> warehouseConfigurationMembers;
        private ObservableCollection<DeliveryMethodConfigurationMember> deliveryMethodConfigurationMembers;
        private ObservableCollection<PaymentMethodConfigurationMember> paymentMethodConfigurationMembers;
        private ObservableCollection<TransactionConfigurationMember> transactionConfigurationMembers;
        private ObservableCollection<BranchConfigurationMember> branchConfigurationMembers;
        private ObservableCollection<TaxRateConfigurationMember> taxRateConfigurationMembers;
        private ObservableCollection<UnitConfigurationMember> unitConfigurationMembers;
        private bool editMode;

        public ICommand AddUnitCommand { get; set; }
        public ICommand AddBranchCommand { get; set; }
        public ICommand AddTaxRateCommand { get; set; }
        public ICommand AddWarehouseCommand { get; set; }
        public ICommand AddTransactionCommand { get; set; }
        public ICommand AddPaymentMethodCommand { get; set; }
        public ICommand AddDeliveryMethodCommand { get; set; }

        public ICommand RemoveUnitCommand { get; set; }
        public ICommand RemoveBranchCommand { get; set; }
        public ICommand RemoveTaxRateCommand { get; set; }
        public ICommand RemoveWarehouseCommand { get; set; }
        public ICommand RemoveTransactionCommand { get; set; }
        public ICommand RemovePaymentMethodCommand { get; set; }
        public ICommand RemoveDeliveryMethodCommand { get; set; }

        public ObservableCollection<WarehouseConfigurationMember> WarehouseConfigurationMembers { get => warehouseConfigurationMembers; set { warehouseConfigurationMembers = value; OnPropertyChanged(); } }
        public ObservableCollection<DeliveryMethodConfigurationMember> DeliveryMethodConfigurationMembers { get => deliveryMethodConfigurationMembers; set { deliveryMethodConfigurationMembers = value; OnPropertyChanged(); } }
        public ObservableCollection<PaymentMethodConfigurationMember> PaymentMethodConfigurationMembers { get => paymentMethodConfigurationMembers; set { paymentMethodConfigurationMembers = value; OnPropertyChanged(); } }
        public ObservableCollection<TransactionConfigurationMember> TransactionConfigurationMembers { get => transactionConfigurationMembers; set { transactionConfigurationMembers = value; OnPropertyChanged(); } }
        public ObservableCollection<BranchConfigurationMember> BranchConfigurationMembers { get => branchConfigurationMembers; set { branchConfigurationMembers = value; OnPropertyChanged(); } }
        public ObservableCollection<TaxRateConfigurationMember> TaxRateConfigurationMembers { get => taxRateConfigurationMembers; set { taxRateConfigurationMembers = value; OnPropertyChanged(); } }
        public ObservableCollection<UnitConfigurationMember> UnitConfigurationMembers { get => unitConfigurationMembers; set { unitConfigurationMembers = value; OnPropertyChanged(); } }

        public ObservableCollection<string> Branches { get; set; }
        public ObservableCollection<string> Warehouses { get; set; }
        public ObservableCollection<string> DeliveryMethods { get; set; }
        public ObservableCollection<string> PaymentMethods { get; set; }
        public ObservableCollection<string> Transactions { get; set; }
        public ObservableCollection<string> Units { get; set; }
        public ObservableCollection<string> TaxRates { get; set; }
        public ObservableCollection<string> Statuses { get; set; }

        public abstract IAccount<T> Account { get; set; }
        public bool EditMode { get => editMode; set { editMode = value; OnPropertyChanged(); } }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public  ICommand AuthenticateCommand { get; set; }
        public  ICommand TestConnectionCommand { get; set; }

        public AccountViewModel(HostViewModel host) : base(host)
        {
            SaveCommand = CreateCommand(Save, (err) => Host.ShowError(err));
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
            CloseCommand = CreateCommand(Close, (err) => Host.ShowError(err));
            EditCommand = CreateCommand(() => EditMode = true, (err) => Host.ShowError(err));
            AuthenticateCommand = CreateAsyncCommand(AuthenticateAsync, (err) => Host.ShowError(err));
            TestConnectionCommand = CreateAsyncCommand(TestConnectionAsync, (err) => Host.ShowError(err));

            AddUnitCommand = CreateCommand(AddUnit);
            AddBranchCommand = CreateCommand(AddBranch);
            AddTaxRateCommand = CreateCommand(AddTaxRate);
            AddWarehouseCommand = CreateCommand(AddWarehouse);
            AddTransactionCommand = CreateCommand(AddTransaction);
            AddPaymentMethodCommand = CreateCommand(AddPaymentMethod);
            AddDeliveryMethodCommand = CreateCommand(AddDeliveryMethod);

            RemoveUnitCommand = CreateCommand<UnitConfigurationMember>(RemoveUnit);
            RemoveBranchCommand = CreateCommand<BranchConfigurationMember>(RemoveBranch);
            RemoveTaxRateCommand = CreateCommand<TaxRateConfigurationMember>(RemoveTaxRate);
            RemoveWarehouseCommand = CreateCommand<WarehouseConfigurationMember>(RemoveWarehouse);
            RemoveTransactionCommand = CreateCommand<TransactionConfigurationMember>(RemoveTransaction);
            RemovePaymentMethodCommand = CreateCommand<PaymentMethodConfigurationMember>(RemovePaymentMethod);
            RemoveDeliveryMethodCommand = CreateCommand<DeliveryMethodConfigurationMember>(RemoveDeliveryMethod);
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
        public void LoadConfigurationMembers()
        {
            DeliveryMethodConfigurationMembers = new ObservableCollection<DeliveryMethodConfigurationMember>(Account.Data.NexoConfiguration.DeliveryMethods);
            PaymentMethodConfigurationMembers = new ObservableCollection<PaymentMethodConfigurationMember>(Account.Data.NexoConfiguration.PaymentMethods);
            TransactionConfigurationMembers = new ObservableCollection<TransactionConfigurationMember>(Account.Data.NexoConfiguration.Transactions);
            WarehouseConfigurationMembers = new ObservableCollection<WarehouseConfigurationMember>(Account.Data.NexoConfiguration.Warehouses);
            TaxRateConfigurationMembers = new ObservableCollection<TaxRateConfigurationMember>(Account.Data.NexoConfiguration.TaxRates);
            BranchConfigurationMembers = new ObservableCollection<BranchConfigurationMember>(Account.Data.NexoConfiguration.Branches);
            UnitConfigurationMembers = new ObservableCollection<UnitConfigurationMember>(Account.Data.NexoConfiguration.Units);
        }
        private void Save()
        {
            Account.Save();
            Account.Dispose();
            (Host.Window.Owner.GetHost().ControlViewModel as AccountsViewModel).LoadAccounts();
            Host.Window.Close();
        }
        private void Cancel()
        {
            var accountsManager = new AccountsManager();
            var original = accountsManager.Find<T>(Account.Data);
            Account.Dispose();

            if (original?.Data == null)
            {
                Host.Window.Close();
                return;
            }

            Account = original;
            LoadConfigurationMembers();
            EditMode = false;
        }
        private void Close()
        {
            Account.Dispose();
            Host.Window.Close();
        }

        protected virtual async Task AuthenticateAsync()
        {
            ISaleChannelService service = null;

            try
            {
                service = Account.Data.CreateMainService();
                var result = await service.AuthenticateAsync();

                if (!result)
                    throw new Exception();

                Host.ShowInfo("Autentykacja przebiegła pomyślnie");
            }
            catch  
            {
                Host.ShowWarning("Autentykacja zakończona niepowodzeniem");
            }
            finally 
            {
                service?.Dispose();
            }
        }
        private async Task TestConnectionAsync()
        {
            ISaleChannelService service = null;

            try
            {
                service = Account.Data.CreateMainService();
                var result = await service.ValidateConnectionAsync();
                Host.ShowInfo("Test połączenia zakończony powodzeniem");
            }
            catch
            {
                Host.ShowWarning("Test połączenia zakończony niepowodzeniem");
            }
            finally
            {
                service?.Dispose();
            }
        }

        private void AddUnit()
        {
            var member = new UnitConfigurationMember();
            UnitConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.Units.Add(member);
        }
        private void AddBranch()
        {
            var member = new BranchConfigurationMember();
            BranchConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.Branches.Add(member);
        }
        private void AddTaxRate()
        {
            var member = new TaxRateConfigurationMember();
            TaxRateConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.TaxRates.Add(member);
        }
        private void AddWarehouse()
        {
            var member = new WarehouseConfigurationMember();
            WarehouseConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.Warehouses.Add(member);
        }
        private void AddTransaction()
        {
            var member = new TransactionConfigurationMember();
            TransactionConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.Transactions.Add(member);
        }
        private void AddPaymentMethod()
        {
            var member = new PaymentMethodConfigurationMember();
            PaymentMethodConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.PaymentMethods.Add(member);
        }
        private void AddDeliveryMethod()
        {
            var member = new DeliveryMethodConfigurationMember();
            DeliveryMethodConfigurationMembers.Add(member);
            Account.Data.NexoConfiguration.DeliveryMethods.Add(member);
        }

        private void RemoveUnit(UnitConfigurationMember member)
        {
            UnitConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.Units.Remove(member);
        }
        private void RemoveBranch(BranchConfigurationMember member)
        {
            BranchConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.Branches.Remove(member);
        }
        private void RemoveTaxRate(TaxRateConfigurationMember member)
        {
            TaxRateConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.TaxRates.Remove(member);
        }
        private void RemoveWarehouse(WarehouseConfigurationMember member)
        {
            WarehouseConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.Warehouses.Remove(member);
        }
        private void RemoveTransaction(TransactionConfigurationMember member)
        {
            TransactionConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.Transactions.Remove(member);
        }
        private void RemovePaymentMethod(PaymentMethodConfigurationMember member)
        {
            PaymentMethodConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.PaymentMethods.Remove(member);
        }
        private void RemoveDeliveryMethod(DeliveryMethodConfigurationMember member)
        {
            DeliveryMethodConfigurationMembers.Remove(member);
            Account.Data.NexoConfiguration.DeliveryMethods.Remove(member);
        }
    }
}


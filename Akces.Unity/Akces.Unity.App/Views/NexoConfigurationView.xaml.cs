using Akces.Unity.App.ViewModels;
using Akces.Unity.Core.NexoData.ConfigurationMembers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Akces.Unity.App.Views
{
    /// <summary>
    /// Logika interakcji dla klasy NexoConfiguration.xaml
    /// </summary>
    public partial class NexoConfigurationView : UserControl
    {
        public NexoConfigurationView()
        {
            InitializeComponent();
        }

        private void AddWarehouseConfigurationMemeber(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).WarehouseConfigurationMembers.Add(new WarehouseConfigurationMember());

        private void AddDeliveryMethodConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).DeliveryMethodConfigurationMembers.Add(new DeliveryMethodConfigurationMember());

        private void AddPaymentMethodConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).PaymentMethodConfigurationMembers.Add(new PaymentMethodConfigurationMember());

        private void AddTransactionConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).TransactionConfigurationMembers.Add(new TransactionConfigurationMember());

        private void AddBranchConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).BranchConfigurationMembers.Add(new BranchConfigurationMember());

        private void AddTaxRateConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).TaxRateConfigurationMembers.Add(new TaxRateConfigurationMember());

        private void AddUnitConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).UnitConfigurationMembers.Add(new UnitConfigurationMember());


        private void RemoveWarehouseConfigurationMemeber(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).WarehouseConfigurationMembers.Remove((sender as Button).DataContext as WarehouseConfigurationMember);

        private void RemoveDeliveryMethodConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).DeliveryMethodConfigurationMembers.Remove((sender as Button).DataContext as DeliveryMethodConfigurationMember);

        private void RemovePaymentMethodConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).PaymentMethodConfigurationMembers.Remove((sender as Button).DataContext as PaymentMethodConfigurationMember);

        private void RemoveTransactionConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).TransactionConfigurationMembers.Remove((sender as Button).DataContext as TransactionConfigurationMember);

        private void RemoveBranchConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).BranchConfigurationMembers.Remove((sender as Button).DataContext as BranchConfigurationMember);

        private void RemoveTaxRateConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).TaxRateConfigurationMembers.Remove((sender as Button).DataContext as TaxRateConfigurationMember);

        private void RemoveUnitConfigurationMember(object sender, RoutedEventArgs e) =>
            (DataContext as NexoConfigurationViewModel).UnitConfigurationMembers.Remove((sender as Button).DataContext as UnitConfigurationMember);
    }
}

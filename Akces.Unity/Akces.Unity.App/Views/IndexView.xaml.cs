using Akces.Unity.App.ViewModels;
using Akces.Unity.Core.SaleChannels;
using System.Windows.Controls;

namespace Akces.Unity.App.Views
{
    public partial class IndexView : UserControl
    {
        public IndexView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = DataContext as IndexViewModel;
            vm.CreateAccount(accountName.Text, (SaleChannelType)accountType.SelectedItem);
        }
    }
}

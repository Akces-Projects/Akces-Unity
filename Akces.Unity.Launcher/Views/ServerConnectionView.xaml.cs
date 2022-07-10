using Akces.Unity.Launcher.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Akces.Unity.Launcher.Views
{
    public partial class ServerConnectionView : UserControl
    {
        public ServerConnectionView()
        {
            InitializeComponent();
            Loaded += ServerConnectionView_Loaded;
        }

        private async void ServerConnectionView_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ServerConnectionViewModel;
            passwordBox.Password = ServerConnectionViewModel.DbPassword;
            await vm.OnStartAsync();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ServerConnectionViewModel.DbPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}

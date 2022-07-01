using Akces.Unity.App.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Akces.Unity.App.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PasswordBox.Focus();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext != null)
            {
                ((LoginViewModel)DataContext).NexoPassword = ((PasswordBox)sender).Password;
            }
        }
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var viewModel = DataContext as LoginViewModel;

                if (!string.IsNullOrEmpty(viewModel.NexoPassword))
                    viewModel.LoginUserCommand.Execute(null);
            }
        }
    }
}

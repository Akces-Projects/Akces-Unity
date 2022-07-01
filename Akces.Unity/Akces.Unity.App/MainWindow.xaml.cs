using Akces.Core.Nexo;
using Akces.Unity.App.ViewModels;
using Akces.Wpf.Extensions;
using Akces.Wpf.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Akces.Unity.App
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var nexoDatabase = ServicesProvider.GetService<NexoDatabase>();
            Title = $"{nexoDatabase.Name} - {App.AppName}";
            DataContext = new MainViewModel(this);

            if (nexoDatabase.NexoConnectionData.AutoLogin)
            {
                var loginViewModel = this.GetHost().UpdateView<LoginViewModel>();
                loginViewModel.SelectedUser = loginViewModel.Users.FirstOrDefault(x => x.Login == nexoDatabase.NexoConnectionData.NexoUsername);
                loginViewModel.NexoPassword = nexoDatabase.NexoConnectionData.NexoPassword;
                loginViewModel.LoginUserCommand.Execute(null);
            }
            else
            {
                this.GetHost().UpdateView<HomeViewModel>();
            }
        }

        public async void RebuildAsync()
        {
            var dc = DataContext;
            loadingOverlay.Visibility = Visibility.Hidden;
            DataContext = null;
            await Task.Delay(10);
            DataContext = dc;
            loadingOverlay.Visibility = Visibility.Visible;
        }
    }
}

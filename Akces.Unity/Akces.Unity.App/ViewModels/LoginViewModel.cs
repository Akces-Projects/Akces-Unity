using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{
    public class LoginViewModel : ControlViewModel
    {
        private NexoUser selectedUser;
        private bool autoLogin;
        private readonly NexoDatabase nexoDatabase;

        public bool AutoLogin { get => autoLogin; set { autoLogin = value; OnAutoLoginChanged(value); } }
        public ObservableCollection<NexoUser> Users { get; set; }
        public ObservableCollection<NexoProduct> NexoProducts { get; set; }
        public NexoUser SelectedUser { get => selectedUser; set { selectedUser = value; OnPropertyChanged(); } }
        public NexoProduct SelectedNexoProduct { get => App.NexoProduct; set { App.NexoProduct = value; } }
        public string NexoPassword { get; set; }
        public ICommand LoginUserCommand { get; set; }
        public ICommand GoToHomeCommand { get; set; }

        public LoginViewModel(HostViewModel host) : base(host)
        {
            var nexoProducts = Enum.GetValues(typeof(NexoProduct)).Cast<NexoProduct>().ToList();
            nexoDatabase = ServicesProvider.GetService<NexoDatabase>();

            AutoLogin = nexoDatabase.NexoConnectionData.AutoLogin;
            Users = new ObservableCollection<NexoUser>(nexoDatabase.GetNexoUsers());
            NexoProducts = new ObservableCollection<NexoProduct>(nexoProducts);
            SelectedUser = Users.FirstOrDefault();
            SelectedNexoProduct = App.NexoProduct;

            nexoDatabase.OnNexoUserLogginFailed += OnNexoUserLogginFailed;
            nexoDatabase.OnNexoUserLoggedIn += OnNexoUserLoggedIn;

            LoginUserCommand = CreateAsyncCommand(LoginUserAsync, null, (arg) => !string.IsNullOrEmpty(NexoPassword), true, "Trwa logowanie...");
            GoToHomeCommand = CreateCommand(GoToHome, (err) => Host.ShowError(err));
        }

        private async Task LoginUserAsync()
        {
            if (SelectedUser == null)
                throw new Exception("Nie wybrano użytkownika");

            await nexoDatabase.LoginAsync(SelectedUser, NexoPassword, SelectedNexoProduct);
        }
        private void GoToHome()
        {
            nexoDatabase.OnNexoUserLogginFailed -= OnNexoUserLogginFailed;
            nexoDatabase.OnNexoUserLoggedIn -= OnNexoUserLoggedIn;
            Host.UpdateView<HomeViewModel>();
        }
        private void OnNexoUserLogginFailed(string error)
        {
            MessageBox.Show(error, "Błąd logowania");
        }
        private void OnNexoUserLoggedIn(NexoContext nexoContext)
        {
            ServicesProvider.AddSingleton(nexoContext);
            ServicesProvider.AddSingleton(new HarmonogramWorker());

            nexoDatabase.OnNexoUserLogginFailed -= OnNexoUserLogginFailed;
            nexoDatabase.OnNexoUserLoggedIn -= OnNexoUserLoggedIn;

            if (AutoLogin)
            {
                nexoDatabase.NexoConnectionData.AutoLogin = true;
                nexoDatabase.NexoConnectionData.NexoUsername = SelectedUser.Login;
                nexoDatabase.NexoConnectionData.NexoPassword = NexoPassword;
                nexoDatabase.ToFile("..\\localdata");
            }

            var navbar = (Host as MainViewModel).NavbarViewModel;
            Host.Window.Title = $"{nexoDatabase.Name} - {nexoContext.NexoUser.Name} - {App.AppName}";
            navbar.Logged = true;
            Host.UpdateView<ActiveHarmonogramViewModel>();
        }
        private void OnAutoLoginChanged(bool value)
        {
            if (value) return;

            nexoDatabase.NexoConnectionData.AutoLogin = false;
            nexoDatabase.NexoConnectionData.NexoUsername = null;
            nexoDatabase.NexoConnectionData.NexoPassword = null;
            nexoDatabase.ToFile("..\\localdata");
        }
    }
}

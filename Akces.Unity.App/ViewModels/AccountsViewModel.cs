using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.ViewModels
{

    public class AccountsViewModel : ControlViewModel 
    {
        private readonly AccountsManager accountsManager;

        public ObservableCollection<Account>Accounts { get; set; }
        public Account SelectedAccount { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }

        public AccountsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.accountsManager = new AccountsManager();
            Accounts = new ObservableCollection<Account>();
            CreateAccountCommand = CreateCommand<AccountType>(CreateAccount, (err) => Host.ShowError(err));
            EditAccountCommand = CreateCommand(EditAccount, (err) => Host.ShowError(err));
            DeleteAccountCommand = CreateCommand(DeleteAccount, (err) => Host.ShowError(err));

            LoadAccounts();
        }

        public void LoadAccounts() 
        {
            var accounts = accountsManager.Get();
            RefreshCollection(Accounts, accounts);
        }
        private void CreateAccount(AccountType accountType) 
        {
            switch (accountType)
            {
                case AccountType.Shoper:
                    OpenAccountEditor<ShoperAccount, ShoperAccountViewModel>();
                    break;
                case AccountType.shopGold:
                    OpenAccountEditor<ShopgoldAccount, ShopgoldAccountViewModel>();
                    break;
                case AccountType.Baselinker:
                    OpenAccountEditor<BaselinkerAccount, BaselinkerAccountViewModel>();
                    break;
                case AccountType.Allegro:
                    OpenAccountEditor<AllegroAccount, AllegroAccountViewModel>();
                    break;
                case AccountType.Olx:
                    OpenAccountEditor<OlxAccount, OlxAccountViewModel>();
                    break;
                default:
                    break;
            }
        }
        private void EditAccount()
        {
            if (SelectedAccount == null)
                return;

            var account = SelectedAccount;

            switch (account.AccountType)
            {
                case AccountType.Shoper:
                    OpenAccountEditor<ShoperAccount, ShoperAccountViewModel>(account);
                    break;
                case AccountType.shopGold:
                    OpenAccountEditor<ShopgoldAccount, ShopgoldAccountViewModel>(account);
                    break;
                case AccountType.Baselinker:
                    OpenAccountEditor<BaselinkerAccount, BaselinkerAccountViewModel>(account);
                    break;
                case AccountType.Allegro:
                    OpenAccountEditor<AllegroAccount, AllegroAccountViewModel>(account);
                    break;
                case AccountType.Olx:
                    OpenAccountEditor<OlxAccount, OlxAccountViewModel>(account);
                    break;
                default:
                    break;
            }
        }
        private void DeleteAccount()
        {
            if (SelectedAccount == null)
                return;

            var account = SelectedAccount;

            var result = MessageBox.Show(
                "Czy na pewno chcesz usunąć konto?", 
                $"{account.Name} ({account.AccountType})", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var accountBO = accountsManager.Find(account);
            accountBO.Delete();
            accountBO.Dispose();

            LoadAccounts();
        }
        private void OpenAccountEditor<T,U>(Account account = null) 
            where T : Account, new()
            where U : AccountViewModel<T> 
        {
            var accountBO = account == null ? accountsManager.Create<T>() : accountsManager.Find<T>(account);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(900, 700);
            var host = window.GetHost();
            var vm = host.UpdateView<U>();
            vm.Account = accountBO;
            vm.LoadNexoOptions();
            vm.LoadConfigurationMembers();
            window.Show();
        }
    }
}

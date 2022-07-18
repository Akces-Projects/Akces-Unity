using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Akces.Unity.App.ViewModels
{
    public class AccountsViewModel : ControlViewModel 
    {
        private readonly AccountsManager accountsManager;
        private List<Account> downloadedAccounts;
        private ObservableCollection<Account> accounts;

        private string searchstring;
        public string Searchstring
        {
            get { return searchstring; }
            set
            {
                searchstring = value;
                OnPropertyChanged();
                OnSearchstringChanged();
            }
        }

        public ObservableCollection<Account> Accounts { get => accounts; set { accounts = value; OnPropertyChanged(); } }
        public Account SelectedAccount { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand ShowAccountCommand { get; set; }
        public ICommand EditAccountCommand { get; set; }
        public ICommand DeleteAccountCommand { get; set; }

        public AccountsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.accountsManager = new AccountsManager();
            CreateAccountCommand = CreateCommand<AccountType>(CreateAccount, (err) => Host.ShowError(err));
            EditAccountCommand = CreateCommand(() => OpenEditor(editMode: true), (err) => Host.ShowError(err));
            ShowAccountCommand = CreateCommand(() => OpenEditor(editMode: false), (err) => Host.ShowError(err));
            DeleteAccountCommand = CreateCommand(DeleteAccount, (err) => Host.ShowWarning(err));
            LoadAccounts();
        }
        public void LoadAccounts() 
        {
            downloadedAccounts = accountsManager.Get();
            Accounts = new ObservableCollection<Account>(downloadedAccounts);
        }
        private void CreateAccount(AccountType accountType) 
        {
            switch (accountType)
            {
                case AccountType.Shoper:
                    OpenAccountEditor<ShoperAccount, ShoperAccountViewModel>(true);
                    break;
                case AccountType.shopGold:
                    OpenAccountEditor<ShopgoldAccount, ShopgoldAccountViewModel>(true);
                    break;
                case AccountType.Baselinker:
                    OpenAccountEditor<BaselinkerAccount, BaselinkerAccountViewModel>(true);
                    break;
                case AccountType.Allegro:
                    OpenAccountEditor<AllegroAccount, AllegroAccountViewModel>(true);
                    break;
                case AccountType.Olx:
                    OpenAccountEditor<OlxAccount, OlxAccountViewModel>(true);
                    break;
                default:
                    break;
            }
        }
        private void OpenEditor(bool editMode)
        {
            if (SelectedAccount == null)
                return;

            var account = SelectedAccount;

            switch (account.AccountType)
            {
                case AccountType.Shoper:
                    OpenAccountEditor<ShoperAccount, ShoperAccountViewModel>(editMode, account);
                    break;
                case AccountType.shopGold:
                    OpenAccountEditor<ShopgoldAccount, ShopgoldAccountViewModel>(editMode, account);
                    break;
                case AccountType.Baselinker:
                    OpenAccountEditor<BaselinkerAccount, BaselinkerAccountViewModel>(editMode, account);
                    break;
                case AccountType.Allegro:
                    OpenAccountEditor<AllegroAccount, AllegroAccountViewModel>(editMode, account);
                    break;
                case AccountType.Olx:
                    OpenAccountEditor<OlxAccount, OlxAccountViewModel>(editMode, account);
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
        private void OpenAccountEditor<T,U>(bool editMode, Account account = null) 
            where T : Account, new()
            where U : AccountViewModel<T> 
        {
            var accountBO = account == null ? accountsManager.Create<T>() : accountsManager.Find<T>(account);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1100, 700);
            var host = window.GetHost();
            var vm = host.UpdateView<U>();
            vm.Account = accountBO;
            vm.EditMode = editMode;
            vm.LoadNexoOptions();
            vm.LoadConfigurationMembers();
            window.Show();
        }
        private void OnSearchstringChanged()
        {
            if (downloadedAccounts == null)
                return;

            List<Account> filteredAccounts = null;

            var searchstring = Searchstring?.ToLower();
            filteredAccounts = downloadedAccounts
                .Where(x => string.IsNullOrEmpty(searchstring) || $"{x.AccountType}{x.Name}".ToLower().Contains(searchstring))
                .ToList();

            if (filteredAccounts == null)
                return;

            Accounts = new ObservableCollection<Account>(filteredAccounts.OrderBy(x => x.Name));
        }
    }
}

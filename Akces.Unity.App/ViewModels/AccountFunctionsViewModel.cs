using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using System.Collections.Generic;
using System.Linq;

namespace Akces.Unity.App.ViewModels
{
    public class AccountFunctionsViewModel : ControlViewModel
    {
        private readonly AccountFunctionsManager accountFunctionsManager;
        private List<AccountFunction> downloadedAccountFunction;
        private ObservableCollection<AccountFunction> accountFunction;

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

        public ObservableCollection<AccountFunction> AccountFunctions { get => accountFunction; set { accountFunction = value; OnPropertyChanged(); } }
        public AccountFunction SelectedAccountFunction { get; set; }
        public ICommand CreateAccountFunctionCommand { get; set; }
        public ICommand ShowAccountFunctionCommand { get; set; }
        public ICommand EditAccountFunctionCommand { get; set; }
        public ICommand DeleteAccountFunctionCommand { get; set; }

        public AccountFunctionsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.accountFunctionsManager = new AccountFunctionsManager();
            CreateAccountFunctionCommand = CreateCommand(CreateAccountFunction, (err) => Host.ShowError(err));
            EditAccountFunctionCommand = CreateCommand(EditAccountFunction, (err) => Host.ShowError(err));
            ShowAccountFunctionCommand = CreateCommand(ShowAccountFunction, (err) => Host.ShowError(err));
            DeleteAccountFunctionCommand = CreateCommand(DeleteAccount, (err) => Host.ShowWarning(err));
            LoadAccountFunctions();
        }
        public void LoadAccountFunctions()
        {
            downloadedAccountFunction = accountFunctionsManager.Get();
            AccountFunctions = new ObservableCollection<AccountFunction>(downloadedAccountFunction);
        }
        private void ShowAccountFunction()
        {
            if (SelectedAccountFunction == null)
                return;

            OpenEditor(editMode: false, SelectedAccountFunction);
        }
        private void CreateAccountFunction() 
        {
            OpenEditor(editMode: true);
        }
        private void EditAccountFunction()
        {
            if (SelectedAccountFunction == null)
                return;

            OpenEditor(editMode: true, SelectedAccountFunction);
        }
        private void OpenEditor(bool editMode, AccountFunction accountFunction = null)
        {
            var accountFunctionBO = accountFunction == null ? accountFunctionsManager.Create() : accountFunctionsManager.Find(accountFunction);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1100, 700);
            var host = window.GetHost();
            var vm = host.UpdateView<AccountFunctionViewModel>();
            vm.AccountFunction = accountFunctionBO;
            vm.EditMode = editMode;
            window.Show();
        }
        private void DeleteAccount()
        {
            if (SelectedAccountFunction == null)
                return;

            var accountFunction = SelectedAccountFunction;

            var result = MessageBox.Show(
                "Czy na pewno chcesz usunąć funkcję?",
                $"{accountFunction.Name} ({accountFunction.Account.Name})",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var accountBO = accountFunctionsManager.Find(accountFunction);
            accountBO.Delete();
            accountBO.Dispose();

            LoadAccountFunctions();
        }
        private void OnSearchstringChanged()
        {
            if (downloadedAccountFunction == null)
                return;

            List<AccountFunction> filteredAccountFunctions = null;

            var searchstring = Searchstring?.ToLower();
            filteredAccountFunctions = downloadedAccountFunction
                .Where(x => string.IsNullOrEmpty(searchstring) || $"{x.Name}".ToLower().Contains(searchstring))
                .ToList();

            if (filteredAccountFunctions == null)
                return;

            AccountFunctions = new ObservableCollection<AccountFunction>(filteredAccountFunctions.OrderBy(x => x.Name));
        }
    }
}

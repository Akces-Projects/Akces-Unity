using System.Windows.Input;
using System.Collections.Generic;
using Akces.Wpf.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using Akces.Wpf.Extensions;
using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using System.Linq;
using System;

namespace Akces.Unity.App.ViewModels
{
    internal class AccountFunctionViewModel : ControlViewModel
    {
        private bool editMode;
        private IAccountFunction accountFunction;

        public IAccountFunction AccountFunction { get => accountFunction; set { accountFunction = value; Init(); OnPropertyChanged(); } }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public bool EditMode { get => editMode; set { editMode = value; OnPropertyChanged(); } }
        public List<AccountFunctionType> AccountFunctionTypes { get; set; }
        public List<Account> Accounts { get; set; }

        public AccountFunctionViewModel(HostViewModel host) : base(host)
        {
            SaveCommand = CreateCommand(Save, (err) => Host.ShowError(err));
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
            CloseCommand = CreateCommand(Close, (err) => Host.ShowError(err));
            EditCommand = CreateCommand(() => EditMode = true, (err) => Host.ShowError(err));
            AccountFunctionTypes = AccountFunctionType.AccountFunctionTypes;
            var accountsManager = new AccountsManager();
            Accounts = accountsManager.Get();
        }

        private void Init() 
        {
            AccountFunction.Data.Account = Accounts.FirstOrDefault(x => x.Id == AccountFunction.Data.Account?.Id);
            AccountFunction.Data.AccountFunctionType = AccountFunctionTypes.FirstOrDefault(x => x.Id == AccountFunction.Data.AccountFunctionType?.Id);
        }

        private void Save()
        {
            AccountFunction.Validate();
            AccountFunction.InitScript();
            AccountFunction.Save();
            AccountFunction.Dispose();

            var user = GetLoggedUnityUser();

            if (!user.IsWorker) 
            {
                Host.ShowInfo("" +
                    "Zalogowany użytkownik nie jest przypisany jako serwer."
                    + Environment.NewLine
                    + Environment.NewLine
                    + "Aby zapisane zmiany zostały wprowadzne konieczny jest restart instancji programu serwera.");
            }

            (Host.Window.Owner.GetHost().ControlViewModel as AccountFunctionsViewModel).LoadAccountFunctions();
            Host.Window.Close();
        }
        private void Cancel()
        {
            var accountFunctionsManager = new AccountFunctionsManager();
            var original = accountFunctionsManager.Find(AccountFunction.Data);
            AccountFunction.Dispose();

            if (original?.Data == null)
            {
                Host.Window.Close();
                return;
            }

            AccountFunction = original;
            EditMode = false;
        }
        private void Close()
        {
            AccountFunction.Dispose();
            Host.Window.Close();
        }
        private UnityUser GetLoggedUnityUser()
        {
            var nexoContext = ServicesProvider.GetService<NexoContext>();
            var unityUsersManager = new UnityUsersManager();
            var unityUser = unityUsersManager.Get().FirstOrDefault(x => x.Login == nexoContext.NexoUser.Login);
            return unityUser;
        }
    }
}

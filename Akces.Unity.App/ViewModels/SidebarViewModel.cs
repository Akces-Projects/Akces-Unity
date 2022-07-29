using System;
using System.Linq;
using System.Windows.Input;
using Akces.Core.Nexo;
using Akces.Unity.DataAccess.Managers;
using Akces.Unity.Models;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;

namespace Akces.Unity.App.ViewModels
{
    public class SidebarViewModel : ControlViewModel 
    {
        public ICommand GoToAccountsCommand { get; set; }
        public ICommand GoToReportsCommand { get; set; }
        public ICommand GoToHarmonogramsCommand { get; set; }
        public ICommand GoToTasks { get; set; }
        public ICommand GoToProductsPrizesUpdateCommand { get; set; }
        public ICommand GoToUnityUsersCommand { get; set; }
        public ICommand GoToAccountFunctionsCommand { get; set; }

        public SidebarViewModel(HostViewModel host) : base(host)
        {
            GoToAccountsCommand = CreateCommand(() => GoTo<AccountsViewModel>(Modules.Accounts), (err) => Host.ShowError(err));
            GoToReportsCommand = CreateCommand(() => GoTo<ReportsViewModel>(Modules.Reports), (err) => Host.ShowError(err));
            GoToHarmonogramsCommand = CreateCommand(() => GoTo<HarmonogramsViewModel>(Modules.Harmonograms), (err) => Host.ShowError(err));
            GoToTasks = CreateCommand(() => GoTo<ActiveHarmonogramViewModel>(Modules.Tasks), (err) => Host.ShowError(err));
            GoToProductsPrizesUpdateCommand = CreateCommand(() => GoTo<ProductsPricesUpdateViewModel>(Modules.Prizes), (err) => Host.ShowError(err));
            GoToUnityUsersCommand = CreateCommand(() => GoTo<UnityUsersViewModel>(Modules.Users), (err) => Host.ShowError(err));
            GoToAccountFunctionsCommand = CreateCommand(() => GoTo<AccountFunctionsViewModel>(Modules.Users), (err) => Host.ShowError(err));
        }

        private void GoTo<T>(string module) where T : ControlViewModel
        {
            var unityUser = GetLoggedUnityUser();
            var authorisation = unityUser.Authorisations.First(x => x.Module == module);

            if (authorisation.AuthorisationType == AuthorisationType.Deny)
                throw new Exception("Brak uprawnień do wybranego zasobu");

            Host.UpdateView<T>();
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

using System.Windows.Input;
using Akces.Core.Nexo;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;

namespace Akces.Unity.App.ViewModels
{
    public class SidebarViewModel : ControlViewModel 
    {
        public ICommand GoToAccountsCommand { get; set; }
        public ICommand GoToReportsCommand { get; set; }
        public ICommand GoToHarmonogramsCommand { get; set; }
        public ICommand GoToActiveHarmonogramCommand { get; set; }
        public ICommand GoToProductsPrizesUpdateCommand { get; set; }

        public SidebarViewModel(HostViewModel host) : base(host)
        {

            GoToAccountsCommand = CreateCommand(() => host.UpdateView<AccountsViewModel>());
            GoToReportsCommand = CreateCommand(() => host.UpdateView<ReportsViewModel>());

            GoToHarmonogramsCommand = CreateCommand(() =>
            {
                var nexoUser = ServicesProvider.GetService<NexoContext>().NexoUser;
                var unityUsersManager = new UnityUsersManager();
                var unityUser = unityUsersManager.Get(nexoUser.Name);

                if (unityUser == null || !unityUser.CanOpenHarmonograms)
                { ShowError(); return; }
                host.UpdateView<HarmonogramsViewModel>();
            }, (err) => ShowError());

            GoToActiveHarmonogramCommand = CreateCommand(() => {

                var nexoUser = ServicesProvider.GetService<NexoContext>().NexoUser;
                var unityUsersManager = new UnityUsersManager();
                var unityUser = unityUsersManager.Get(nexoUser.Name);

                if (unityUser == null || !unityUser.CanOpenHarmonograms)
                { ShowError(); return; }
                host.UpdateView<ActiveHarmonogramViewModel>();
            }, (err) => ShowError());

            GoToProductsPrizesUpdateCommand = CreateCommand(() => {

                var nexoUser = ServicesProvider.GetService<NexoContext>().NexoUser;
                var unityUsersManager = new UnityUsersManager();
                var unityUser = unityUsersManager.Get(nexoUser.Name);

                if (unityUser == null || !unityUser.CanOpenHarmonograms)
                { ShowError(); return; }
                host.UpdateView<ProductsPricesUpdateViewModel>();
            }, (err) => ShowError());
        }

        private void ShowError() 
        {
            Host.ShowWarning("Brak uprawnień");
        }
    }
}

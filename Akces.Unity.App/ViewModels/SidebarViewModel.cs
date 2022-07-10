using System.Windows.Input;
using Akces.Core.Nexo;
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
        public ICommand GoToTasks { get; set; }
        public ICommand GoToProductsPrizesUpdateCommand { get; set; }

        public SidebarViewModel(HostViewModel host) : base(host)
        {
            GoToAccountsCommand = CreateCommand(() => host.UpdateView<AccountsViewModel>());
            GoToReportsCommand = CreateCommand(() => host.UpdateView<ReportsViewModel>());
            GoToHarmonogramsCommand = CreateCommand(() => host.UpdateView<HarmonogramsViewModel>());
            GoToTasks = CreateCommand(() => host.UpdateView<ActiveHarmonogramViewModel>());
            GoToProductsPrizesUpdateCommand = CreateCommand(() => host.UpdateView<ProductsPricesUpdateViewModel>());
        }

        public void GoToAccounts() 
        {

        }

        private void ShowError() 
        {
            Host.ShowWarning("Brak uprawnień");
        }
    }
}

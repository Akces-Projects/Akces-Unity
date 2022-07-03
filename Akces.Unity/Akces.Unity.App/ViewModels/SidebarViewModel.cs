using System.Windows.Input;
using Akces.Wpf.Models;

namespace Akces.Unity.App.ViewModels
{
    public class SidebarViewModel : ControlViewModel 
    {
        public ICommand GoToAccountsCommand { get; set; }
        public ICommand GoToReportsCommand { get; set; }
        public ICommand GoToHarmonogramsCommand { get; set; }
        public ICommand GoToActiveHarmonogramCommand { get; set; }

        public SidebarViewModel(HostViewModel host) : base(host)
        {
            GoToAccountsCommand = CreateCommand(() => host.UpdateView<AccountsViewModel>());
            GoToReportsCommand = CreateCommand(() => host.UpdateView<ReportsViewModel>());
            GoToHarmonogramsCommand = CreateCommand(() => host.UpdateView<HarmonogramsViewModel>());
            GoToActiveHarmonogramCommand = CreateCommand(() => host.UpdateView<ActiveHarmonogramViewModel>());
        }
    }
}

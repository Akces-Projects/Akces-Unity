using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{
    public class NavbarViewModel : ControlViewModel
    {
        private bool logged;
        public ICommand OpenHelpFileCommand { get; set; }
        public ICommand ShowContactCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public bool Logged { get => logged; set { logged = value; OnPropertyChanged(); } }

        public NavbarViewModel(HostViewModel host) : base(host)
        {
            ShowContactCommand = CreateCommand(() => Host.ShowInfo(App.Contact, "Kontakt"));
            OpenHelpFileCommand = CreateCommand(() => Process.Start(App.HelpFileLocation), (err) => Host.ShowInfo(HelpInfo + Environment.NewLine + Environment.NewLine + Environment.NewLine + App.Contact, "Pomoc"));
            LogoutCommand = CreateCommand(Logout, (err) => Host.ShowError(err));
        }

        private void Logout()
        {
            ServicesProvider.RemoveInstance<HarmonogramWorker>()?.Dispose();
            ServicesProvider.RemoveInstance<NexoContext>()?.Dispose();

            var nexoDatabase = ServicesProvider.GetService<NexoDatabase>();
            Host.Window.Title = $"{nexoDatabase.Name} - {App.AppName}";
            Host.UpdateView<LoginViewModel>();
            (Host as MainViewModel).SidebarVisable = false;
            Logged = false;
        }

        private const string HelpInfo = "Ta aplikacja nie posiada szczegółowej instrukcji użytkowania. W przypadku pytań dotyczących jej funkcjonalności lub dodatkowych informacji uprzejmie prosimy o kontakt.";
    }
}

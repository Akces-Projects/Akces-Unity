using Akces.Wpf.Models;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{

    public class HomeViewModel : ControlViewModel
    {
        public ICommand StartLauncherCommand { get; set; }
        public ICommand GoToLoginCommand { get; set; }

        public HomeViewModel(HostViewModel host) : base(host)
        {
            StartLauncherCommand = CreateCommand(StartLauncher, (err) => Host.ShowError(err));
            GoToLoginCommand = CreateCommand(() => Host.UpdateView<LoginViewModel>(), (err) => Host.ShowError(err));
        }

        private void StartLauncher()
        {
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo(App.LauncherName, "settings")
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    WorkingDirectory = Path.GetDirectoryName($"..\\{App.LauncherName}")
                }
            };

            process.Start();
            Application.Current.Shutdown();
        }
    }
}

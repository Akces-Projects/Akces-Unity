using Akces.Unity.Core.NexoData.Operations;
using Akces.Wpf.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{
    public class RaportViewModel : ControlViewModel
    {
        public ObservableCollection<OperationResult> OperationResults { get; }

        public RaportViewModel(HostViewModel host) : base(host)
        {
            OperationResults = new ObservableCollection<OperationResult>();
        }
    }
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

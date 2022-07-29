using Akces.Wpf.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Akces.Unity.Launcher.ViewModels
{
    public class OperationsProgressViewModel : ControlViewModel
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        private int currenPosition;
        private int positionsCount;
        public int CurrentPosition
        {
            get { return currenPosition; }
            set { currenPosition = value; OnPropertyChanged(); }
        }
        public int PositionsCount
        {
            get { return positionsCount; }
            set { positionsCount = value; OnPropertyChanged(); }
        }

        public AppUpdater AppUpdater { get; set; }
        public ICommand CancelOperationsCommand { get; set; }


        public OperationsProgressViewModel(HostViewModel host) : base(host)
        {
            Host.Window.Closed += (s, e) => cancellationTokenSource.Cancel();
            cancellationTokenSource = new CancellationTokenSource();
            CancelOperationsCommand = CreateCommand(() => AppUpdater.CancelAppUpdate());
        }

        public async Task RunOperationsAsync()
        {
            PositionsCount = 100;
            AppUpdater.OnAppUpdateProgress += OnOperationProgress;
            await AppUpdater.UpdateAsync();
            Host.Window.Close();
        }
        private void OnOperationProgress(int progressPercentage)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                CurrentPosition = progressPercentage;
            }));
        }
    }
}

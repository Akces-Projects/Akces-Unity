using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using Akces.Wpf.Helpers;
using System.Windows.Input;
using System.Windows.Threading;
using System;
using Akces.Wpf.Extensions;

namespace Akces.Unity.App.ViewModels
{
    internal class ActiveHarmonogramViewModel : ControlViewModel
    {
        private readonly HarmonogramWorker harmonogramWorker;
        private readonly WorkerStatusesManager workerStatusesManager = new WorkerStatusesManager();
        private readonly HarmonogramsManager harmonogramsManager = new HarmonogramsManager();
        private readonly TaskReportsManager reportsManager = new TaskReportsManager();
        private int? currentWorkingPositionId;
        private bool isWorkerEnabled;

        public ObservableCollection<HarmonogramPosition> Positions { get; set; } = new ObservableCollection<HarmonogramPosition>();
        public HarmonogramPosition  SelectedPosition { get; set; }
        public int? CurrentWorkingPositionId { get => currentWorkingPositionId; set { currentWorkingPositionId = value; OnPropertyChanged(); } }
        public bool IsWorkerEnabled
        { 
            get => isWorkerEnabled; 
            set 
            { 
                isWorkerEnabled = value; 
                OnPropertyChanged(); 
                OnPropertyChanged(nameof(IsWorkerNotEnabled)); 
            } 
        }
        public bool IsWorkerNotEnabled { get => !isWorkerEnabled; }
        public Harmonogram ActiveHarmonogram { get; set; }
        public ICommand StartWorkerCommand { get; set; }
        public ICommand StopWorkerCommand { get; set; }
        public ICommand ShowReportCommand { get; set; }

        public ActiveHarmonogramViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            harmonogramWorker = ServicesProvider.GetService<HarmonogramWorker>();
            IsWorkerEnabled = workerStatusesManager.GetCurrent()?.Enabled ?? false;

            harmonogramWorker.OnOperationStarted += OnOperationStarted;
            harmonogramWorker.OnOperationFinished += OnOperationFinished;
            StartWorkerCommand = CreateCommand(StartWorker, (err) => host.ShowError(err));
            StopWorkerCommand = CreateCommand(StopWorker, (err) => host.ShowError(err));
            ShowReportCommand = CreateCommand(ShowReport, (err) => host.ShowError(err));

            LoadHarmonogramPositions();
        }

        private void LoadHarmonogramPositions()
        {
            ActiveHarmonogram = harmonogramsManager.GetActive();

            if (ActiveHarmonogram == null)
                return;

            var activePositions = ActiveHarmonogram.Positions.Where(x => x.Active).ToList();
            RefreshCollection(Positions, activePositions);
        }
        private void StartWorker()
        {
            harmonogramWorker.Start();
            IsWorkerEnabled = harmonogramWorker.Enabled;
        }
        private void StopWorker()
        {
            harmonogramWorker.Stop();
            IsWorkerEnabled = harmonogramWorker.Enabled;
        }
        private void ShowReport()
        {
            if (SelectedPosition == null)
                return;

            var reports = reportsManager.GetForHarmonogramPosition(SelectedPosition);

            if (reports == null || !reports.Any())
                return;

            var report = reports.OrderBy(x => x.Created).LastOrDefault();
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1100, 700);
            var host = window.GetHost();
            var vm = host.UpdateView<ReportViewModel>();
            vm.Report = report;
            window.Show();
        }

        private void OnOperationStarted(HarmonogramPosition harmonogramPosition) 
        {
            CurrentWorkingPositionId = harmonogramPosition?.Id;
        }
        private void OnOperationFinished(TaskReport report, HarmonogramPosition harmonogramPosition) 
        {
            CurrentWorkingPositionId = null;

            var position = Positions.FirstOrDefault(x => x.Id == harmonogramPosition.Id);

            if (position == null)
                return;
            
            position.LastLaunchTime = harmonogramPosition.LastLaunchTime;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                RefreshCollection(Positions)));
        }
    }
}

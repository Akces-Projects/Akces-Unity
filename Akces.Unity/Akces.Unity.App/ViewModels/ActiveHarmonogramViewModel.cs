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

namespace Akces.Unity.App.ViewModels
{
    internal class ActiveHarmonogramViewModel : ControlViewModel
    {
        private readonly HarmonogramWorker harmonogramWorker;
        private readonly HarmonogramsManager harmonogramsManager;
        private int? currentWorkingPositionId;
        private bool isWorkerEnabled;

        private Harmonogram activeHarmonogram;
        public Harmonogram ActiveHarmonogram 
        { 
            get => activeHarmonogram;
            set 
            {
                if (value != activeHarmonogram)
                {
                    OnActiveHarmonogramSelected(value);
                    OnPropertyChanged();
                }
            } 
        }

        public ObservableCollection<HarmonogramPosition> Positions { get; set; }
        public ObservableCollection<Harmonogram> Harmonograms { get; set; }
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
        public ICommand StartWorkerCommand { get; set; }
        public ICommand StopWorkerCommand { get; set; }

        public ActiveHarmonogramViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            harmonogramWorker = ServicesProvider.GetService<HarmonogramWorker>();
            harmonogramWorker.OnOperationStarted += OnOperationStarted;
            harmonogramWorker.OnOperationExecuted += OnOperationExecuted;
            harmonogramsManager = new HarmonogramsManager();
            Positions = new ObservableCollection<HarmonogramPosition>();
            Harmonograms = new ObservableCollection<Harmonogram>(harmonogramsManager.Get());
            activeHarmonogram = Harmonograms.FirstOrDefault(x => x.Active);
            IsWorkerEnabled = harmonogramWorker.Enabled;
            StartWorkerCommand = CreateCommand(StartWorker, (err) => host.ShowError(err));
            StopWorkerCommand = CreateCommand(StopWorker, (err) => host.ShowError(err));
            LoadHarmonogramPositions(activeHarmonogram);
        }

        private void LoadHarmonogramPositions(Harmonogram harmonogram)
        {
            if (harmonogram == null)
                return;

            var fullHarmonogram = harmonogramsManager.Get(harmonogram.Id);
            var activePositions = fullHarmonogram.Positions.Where(x => x.Active).ToList();
            RefreshCollection(Positions, activePositions);
        }
        private void OnActiveHarmonogramSelected(Harmonogram harmonogram) 
        {
            var result = MessageBox.Show(
                $"Czy ustawić harmonogram {harmonogram.Name} jako aktywny?",
                "Aktywny harmonogram",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
                return;

            harmonogramWorker.SetActiveHarmonogram(harmonogram);

            LoadHarmonogramPositions(harmonogram);
            activeHarmonogram = harmonogram;
            harmonogramWorker.Enabled = activeHarmonogram.WorkerEnabled;
        }
        private void StartWorker()
        {
            harmonogramWorker.Enabled = true;
            activeHarmonogram.WorkerEnabled = true;

            using (var harmonogramBO = harmonogramsManager.Find(activeHarmonogram))
            {
                harmonogramBO.Data.WorkerEnabled = true;
                harmonogramBO.Save();
            }

            IsWorkerEnabled = true;
        }
        private void StopWorker()
        {
            harmonogramWorker.Enabled = false;
            activeHarmonogram.WorkerEnabled = false;

            using (var harmonogramBO = harmonogramsManager.Find(activeHarmonogram))
            {
                harmonogramBO.Data.WorkerEnabled = false;
                harmonogramBO.Save();
            }

            IsWorkerEnabled = false;
        }

        private void OnOperationStarted(HarmonogramPosition harmonogramPosition) 
        {
            CurrentWorkingPositionId = harmonogramPosition?.Id;
        }
        private void OnOperationExecuted(HarmonogramPosition harmonogramPosition, OperationReport report) 
        {
            if (CurrentWorkingPositionId == harmonogramPosition.Id) 
                CurrentWorkingPositionId = null;

            var position = Positions.FirstOrDefault(x => x.Id == harmonogramPosition.Id);

            if (position != null) 
            {
                position.LastLaunchTime = harmonogramPosition.LastLaunchTime;

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => 
                    RefreshCollection(Positions)));
            }
        }
    }
}

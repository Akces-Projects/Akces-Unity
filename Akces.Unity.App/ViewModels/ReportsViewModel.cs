using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using System.Collections.Generic;
using System.Linq;
using Akces.Unity.App.Operations;
using System.Threading.Tasks;

namespace Akces.Unity.App.ViewModels
{
    public class ReportsViewModel : ControlViewModel
    {
        private string searchstring;
        private readonly TaskReportsManager reportsManager;
        private List<TaskReport> downloadedReports;
        private ObservableCollection<TaskReport> reports;

        public ObservableCollection<TaskReport> Reports { get => reports; set { reports = value; OnPropertyChanged(); } }
        public List<TaskReport> SelectedReports { get; set; }
        public string Searchstring
        {
            get { return searchstring; }
            set
            {
                searchstring = value;
                OnPropertyChanged();
                OnSearchstringChanged();
            }
        }
        public ICommand ShowReportCommand { get; set; }
        public ICommand DeleteReportCommand { get; set; }

        public ReportsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.reportsManager = new TaskReportsManager();
            Reports = new ObservableCollection<TaskReport>();
            SelectedReports = new List<TaskReport>();
            ShowReportCommand = CreateCommand(ShowReport, (err) => Host.ShowError(err));
            DeleteReportCommand = CreateAsyncCommand(DeleteReportsAsync, (err) => Host.ShowError(err), null, true, "Usuwanie raportów...");
            LoadReports();
        }

        private void LoadReports()
        {
            downloadedReports = reportsManager.Get();
            Reports = new ObservableCollection<TaskReport>(downloadedReports.OrderByDescending(x => x.Created));
        }
        private void ShowReport()
        {
            if (SelectedReports == null || !SelectedReports.Any())
                return;

            var report = reportsManager.Get(SelectedReports.First().Id);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1300, 700);
            var host = window.GetHost();
            var vm = host.UpdateView<ReportViewModel>();
            vm.Report = report;
            window.Show();
        }
        private async Task DeleteReportsAsync()
        {
            if (SelectedReports == null || !SelectedReports.Any())
                return;

            var reports = SelectedReports.ToList();

            var result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć ({reports.Count}) raporty?",
                $"Raporty",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var deleteTaskReportsTask = new DeleteTaskReportsTask(reports);

            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(700, 150);
            var vm = window.GetHost().UpdateView<OperationsProgressViewModel>();
            vm.Operation = deleteTaskReportsTask;
            window.Title = $"Usuwanie raportów";
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.ResizeMode = ResizeMode.NoResize;
            window.Show();
            await vm.RunOperationsAsync();
            LoadReports();
        }
        private void OnSearchstringChanged()
        {
            if (downloadedReports == null)
                return;

            List<TaskReport> filteredReports = null;

            var searchstring = Searchstring?.ToLower();
            filteredReports = downloadedReports.Where(x => string.IsNullOrEmpty(searchstring) || x.Description.ToLower().Contains(searchstring)).ToList();

            if (filteredReports == null)
                return;

            Reports = new ObservableCollection<TaskReport>(filteredReports.OrderByDescending(x => x.Created));
        }
    }
}

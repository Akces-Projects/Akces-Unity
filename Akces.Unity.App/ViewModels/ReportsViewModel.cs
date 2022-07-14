using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using System.Collections.Generic;
using System.Linq;

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
            DeleteReportCommand = CreateCommand(DeleteReport, (err) => Host.ShowError(err));

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
        private void DeleteReport()
        {
            if (SelectedReports == null || !SelectedReports.Any())
                return;

            var reports = SelectedReports.ToArray();

            var result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć ({reports.Length}) raporty?",
                $"Raporty",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            foreach (var report in reports)
            {
                var reportBO = reportsManager.Find(report);
                reportBO.Delete();
                reportBO.Dispose();
            }

            foreach (var report in reports)
            {
                Reports.Remove(report);
            }
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

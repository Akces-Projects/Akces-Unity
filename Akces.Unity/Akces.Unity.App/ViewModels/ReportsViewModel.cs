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
        private readonly OperationReportsManager reportsManager;

        public ObservableCollection<OperationReport> Reports { get; set; }
        public List<OperationReport> SelectedReports { get; set; }
        public ICommand ShowReportCommand { get; set; }
        public ICommand DeleteReportCommand { get; set; }

        public ReportsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.reportsManager = new OperationReportsManager();
            Reports = new ObservableCollection<OperationReport>();
            SelectedReports = new List<OperationReport>();

            ShowReportCommand = CreateCommand(ShowReport, (err) => Host.ShowError(err));
            DeleteReportCommand = CreateCommand(DeleteReport, (err) => Host.ShowError(err));

            LoadReports();
        }

        private void LoadReports()
        {
            var reports = reportsManager.Get();
            RefreshCollection(Reports, reports);
        }
        private void ShowReport()
        {
            if (SelectedReports == null || !SelectedReports.Any())
                return;

            var report = reportsManager.Get(SelectedReports.First().Id);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1100, 700);
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
    }
}

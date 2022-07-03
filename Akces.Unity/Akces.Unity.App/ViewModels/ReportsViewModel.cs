using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.ViewModels
{
    public class ReportsViewModel : ControlViewModel
    {
        private readonly OperationReportsManager reportsManager;

        public ObservableCollection<OperationReport> Reports { get; set; }
        public OperationReport SelectedReport { get; set; }
        public ICommand ShowReportCommand { get; set; }
        public ICommand DeleteReportCommand { get; set; }

        public ReportsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.reportsManager = new OperationReportsManager();
            Reports = new ObservableCollection<OperationReport>();

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
            if (SelectedReport == null)
                return;

            var report = reportsManager.Get(SelectedReport.Id);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(800, 600);
            var host = window.GetHost();
            var vm = host.UpdateView<ReportViewModel>();
            vm.Report = report;
            window.Show();
        }
        private void DeleteReport()
        {
            if (SelectedReport == null)
                return;

            var report = SelectedReport;

            var result = MessageBox.Show(
                "Czy na pewno chcesz usunąć raport?",
                $"{report.Description} ({report.Created})",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var reportBO = reportsManager.Find(report);
            reportBO.Delete();
            reportBO.Dispose();

            Reports.Remove(SelectedReport);
        }
    }
}

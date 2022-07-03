using Akces.Unity.Models;
using Akces.Wpf.Models;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{
    internal class ReportViewModel : ControlViewModel
    {
        private OperationReport report;

        public OperationReport Report { get => report; set { report = value; OnPropertyChanged(); } }

        public ICommand CancelCommand { get; set; }

        public ReportViewModel(HostViewModel host) : base(host)
        {
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
        }

        private void Cancel()
        {
            Host.Window.Close();
        }
    }
}

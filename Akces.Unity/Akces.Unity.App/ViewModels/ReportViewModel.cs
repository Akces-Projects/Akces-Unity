﻿using Akces.Unity.Models;
using Akces.Wpf.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Akces.Unity.App.ViewModels
{
    internal class ReportViewModel : ControlViewModel
    {
        private OperationReport report;
        private string searchstring;

        public OperationReport Report { get => report; set { report = value; OnPropertyChanged(); OnSearchChanged(null); } }
        public ObservableCollection<OperationReportPosition> Positions { get; set; }
        public string Searchstring { set { searchstring = value; OnSearchChanged(value); } }

        public ICommand CancelCommand { get; set; }

        public ReportViewModel(HostViewModel host) : base(host)
        {
            Positions = new ObservableCollection<OperationReportPosition>();
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
        }

        private void Cancel()
        {
            Host.Window.Close();
        }

        private void OnSearchChanged(string value) 
        {
            value = value?.ToLower();

            var positions = Report.Positions
                .Where(x => 
                string.IsNullOrEmpty(value) || 
                (x.ObjectName != null && x.ObjectName.ToLower().Contains(value)) || 
                (x.Description != null && x.Description.ToLower().Contains(value)))
                .ToList();

            RefreshCollection(Positions, positions);
        }
    }
}

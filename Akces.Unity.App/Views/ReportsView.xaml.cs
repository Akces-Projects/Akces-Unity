using Akces.Unity.App.ViewModels;
using Akces.Unity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Akces.Unity.App.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ReportsView.xaml
    /// </summary>
    public partial class ReportsView : UserControl
    {
        private readonly List<TaskReport> operationReports = new List<TaskReport>();

        public ReportsView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = (DataContext as ReportsViewModel);

            if (vm == null)
                return;

            var operationReports = vm.SelectedReports;
            operationReports.AddRange(e.AddedItems.Cast<TaskReport>());

            foreach (var report in e.RemovedItems.Cast<TaskReport>())
            {
                operationReports.Remove(report);
            }
        }
    }
}

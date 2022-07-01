using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Akces.Unity.App.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(App.WebsiteLink));
            e.Handled = true;
        }
    }
}

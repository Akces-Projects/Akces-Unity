using Akces.Unity.Launcher.ViewModels;
using Akces.Wpf.Extensions;
using System.Windows;

namespace Akces.Unity.Launcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(this);
            this.GetHost().UpdateView<ServerConnectionViewModel>();
        }
    }
}

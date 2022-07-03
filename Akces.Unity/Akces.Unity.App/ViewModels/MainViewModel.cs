using System.Windows;
using Akces.Wpf.Models;

namespace Akces.Unity.App.ViewModels
{
    public class MainViewModel : HostViewModel
    {
        private bool sidebarVisable;

        public NavbarViewModel NavbarViewModel { get; set; }
        public SidebarViewModel SidebarViewModel { get; set; }
        public bool SidebarVisable { get => sidebarVisable; set { sidebarVisable = value; OnPropertyChanged(); } }

        public MainViewModel(Window window) : base(window)
        {
            NavbarViewModel = new NavbarViewModel(this);
            SidebarViewModel = new SidebarViewModel(this);
        }
    }
}

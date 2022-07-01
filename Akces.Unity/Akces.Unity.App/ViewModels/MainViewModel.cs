using System.Windows;
using Akces.Wpf.Models;

namespace Akces.Unity.App.ViewModels
{
    public class MainViewModel : HostViewModel
    {
        public NavbarViewModel NavbarViewModel { get; set; }
        public MainViewModel(Window window) : base(window)
        {
            NavbarViewModel = new NavbarViewModel(this);
        }
    }
}

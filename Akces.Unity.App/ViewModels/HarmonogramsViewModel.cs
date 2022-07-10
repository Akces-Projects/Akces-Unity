using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.ViewModels
{
    public class HarmonogramsViewModel : ControlViewModel
    {
        private readonly HarmonogramsManager harmonogramsManager;

        public ObservableCollection<Harmonogram> Harmonograms { get; set; }
        public Harmonogram SelectedHarmonogram { get; set; }
        public ICommand CreateHarmonogramCommand { get; set; }
        public ICommand EditHarmonogramCommand { get; set; }
        public ICommand DeleteHarmonogramCommand { get; set; }

        public HarmonogramsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.harmonogramsManager = new HarmonogramsManager();
            Harmonograms = new ObservableCollection<Harmonogram>();
            CreateHarmonogramCommand = CreateCommand(CreateHarmonogram, (err) => Host.ShowError(err));
            EditHarmonogramCommand = CreateCommand(EditHarmonogram, (err) => Host.ShowError(err));
            DeleteHarmonogramCommand = CreateCommand(DeleteHarmonogram, (err) => Host.ShowError(err));

            LoadHarmonograms();
        }

        public void LoadHarmonograms()
        {
            var harmonograms = harmonogramsManager.Get();
            RefreshCollection(Harmonograms, harmonograms);
        }
        private void CreateHarmonogram()
        {
            OpenHarmonogramEditor();
        }
        private void EditHarmonogram()
        {
            if (SelectedHarmonogram == null)
                return;

            OpenHarmonogramEditor(SelectedHarmonogram);
        }
        private void DeleteHarmonogram()
        {
            if (SelectedHarmonogram == null)
                return;

            var harmonogram = SelectedHarmonogram;

            var result = MessageBox.Show(
                "Czy na pewno chcesz usunąć harmonogram?",
                $"{harmonogram.Name}",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
                return;

            var harmonogramBO = harmonogramsManager.Find(harmonogram);
            harmonogramBO.Delete();
            harmonogramBO.Dispose();

            LoadHarmonograms();
        }
        private void OpenHarmonogramEditor(Harmonogram harmonogram = null)
        {
            var harmonogramBO = harmonogram == null ? harmonogramsManager.Create() : harmonogramsManager.Find(harmonogram);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(1300, 600);
            var host = window.GetHost();
            var vm = host.UpdateView<HarmonogramViewModel>();
            vm.Harmonogram = harmonogramBO;
            vm.LoadHarmonogramPositions();
            window.Show();
        }
    }
}

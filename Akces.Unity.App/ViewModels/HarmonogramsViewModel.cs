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
    public class HarmonogramsViewModel : ControlViewModel
    {
        private readonly HarmonogramsManager harmonogramsManager; 
        private List<Harmonogram> downloadedHarmonograms;
        private ObservableCollection<Harmonogram> harmonogram;

        private string searchstring;
        public string Searchstring
        {
            get { return searchstring; }
            set
            {
                searchstring = value;
                OnPropertyChanged();
                OnSearchstringChanged();
            }
        }

        public ObservableCollection<Harmonogram> Harmonograms { get => harmonogram; set { harmonogram = value; OnPropertyChanged(); } }
        public Harmonogram SelectedHarmonogram { get; set; }
        public ICommand CreateHarmonogramCommand { get; set; }
        public ICommand EditHarmonogramCommand { get; set; }
        public ICommand DeleteHarmonogramCommand { get; set; }

        public HarmonogramsViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            this.harmonogramsManager = new HarmonogramsManager();
            CreateHarmonogramCommand = CreateCommand(CreateHarmonogram, (err) => Host.ShowError(err));
            EditHarmonogramCommand = CreateCommand(EditHarmonogram, (err) => Host.ShowError(err));
            DeleteHarmonogramCommand = CreateCommand(DeleteHarmonogram, (err) => Host.ShowError(err));
            LoadHarmonograms();
        }

        public void LoadHarmonograms()
        {
            downloadedHarmonograms = harmonogramsManager.Get();
            Harmonograms = new ObservableCollection<Harmonogram>(downloadedHarmonograms);
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
        private void OnSearchstringChanged()
        {
            if (downloadedHarmonograms == null)
                return;

            List<Harmonogram> filteredHarmonograms = null;

            var searchstring = Searchstring?.ToLower();
            filteredHarmonograms = downloadedHarmonograms
                .Where(x => string.IsNullOrEmpty(searchstring) || $"{x.Name}".ToLower().Contains(searchstring))
                .ToList();

            if (filteredHarmonograms == null)
                return;

            Harmonograms = new ObservableCollection<Harmonogram>(filteredHarmonograms.OrderBy(x => x.Name));
        }
    }
}

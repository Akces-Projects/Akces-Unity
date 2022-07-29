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
    public class UnityUsersViewModel : ControlViewModel
    {
        private readonly UnityUsersManager unityUserManager;
        private List<UnityUser> downloadedUnityUsers;
        private ObservableCollection<UnityUser> harmonogramUnityUsers;

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

        public ObservableCollection<UnityUser> UnityUsers { get => harmonogramUnityUsers; set { harmonogramUnityUsers = value; OnPropertyChanged(); } }
        public UnityUser SelectedUnityUser { get; set; }
        public ICommand EditUnityUserCommand { get; set; }
        public ICommand ShowUnityUserCommand { get; set; }

        public UnityUsersViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            unityUserManager = new UnityUsersManager();
            EditUnityUserCommand = CreateCommand(() => OpenEditor(editMode: true), (err) => Host.ShowError(err));
            ShowUnityUserCommand = CreateCommand(() => OpenEditor(editMode: false), (err) => Host.ShowError(err));
            LoadUnityUsers();
        }

        public void LoadUnityUsers() 
        {
            downloadedUnityUsers = unityUserManager.Get();
            UnityUsers = new ObservableCollection<UnityUser>(downloadedUnityUsers);
        }
        private void OpenEditor(bool editMode)
        {
            if (SelectedUnityUser == null)
                return;

            var unityUserBO = unityUserManager.Find(SelectedUnityUser);
            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(900, 700);
            var host = window.GetHost();
            var vm = host.UpdateView<UnityUserViewModel>();
            vm.UnityUser = unityUserBO;
            vm.EditMode = editMode;
            window.Show();
        }
        private void OnSearchstringChanged()
        {
            if (downloadedUnityUsers == null)
                return;

            List<UnityUser> filteredUnityUsers = null;

            var searchstring = Searchstring?.ToLower();
            filteredUnityUsers = downloadedUnityUsers
                .Where(x => string.IsNullOrEmpty(searchstring) || $"{x.Name}{x.Login}".ToLower().Contains(searchstring))
                .ToList();

            if (filteredUnityUsers == null)
                return;

            UnityUsers = new ObservableCollection<UnityUser>(filteredUnityUsers.OrderBy(x => x.Name));
        }
    }
}

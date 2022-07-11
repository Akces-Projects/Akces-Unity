using System.Windows.Input;
using System.Collections.ObjectModel;
using Akces.Wpf.Models;
using Akces.Wpf.Extensions;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.ViewModels
{
    public class UnityUsersViewModel : ControlViewModel
    {
        private readonly UnityUsersManager unityUserManager;

        public ObservableCollection<UnityUser> UnityUsers { get; set; }
        public UnityUser SelectedUnityUser { get; set; }
        public ICommand EditUnityUserCommand { get; set; }
        public ICommand ShowUnityUserCommand { get; set; }

        public UnityUsersViewModel(HostViewModel host) : base(host)
        {
            (Host as MainViewModel).SidebarVisable = true;
            unityUserManager = new UnityUsersManager();
            UnityUsers = new ObservableCollection<UnityUser>();
            EditUnityUserCommand = CreateCommand(() => OpenEditor(editMode: true), (err) => Host.ShowError(err));
            ShowUnityUserCommand = CreateCommand(() => OpenEditor(editMode: false), (err) => Host.ShowError(err));

            LoadUnityUsers();
        }

        public void LoadUnityUsers()
        {
            var unityUsers = unityUserManager.Get();
            RefreshCollection(UnityUsers, unityUsers);
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
    }
}

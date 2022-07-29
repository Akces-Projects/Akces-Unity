using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using Akces.Wpf.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;
using Akces.Wpf.Extensions;
using Akces.Wpf.Helpers;
using Akces.Core.Nexo;
using System.Windows;

namespace Akces.Unity.App.ViewModels
{
    internal class UnityUserViewModel : ControlViewModel
    {
        private bool editMode;
        private bool wasWorker;
        private IUnityUser unityUser;

        public IUnityUser UnityUser { get => unityUser; set { unityUser = value; wasWorker = unityUser.Data.IsWorker; OnPropertyChanged(); } }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public bool EditMode { get => editMode; set { editMode = value; OnPropertyChanged(); } }
        public List<AuthorisationType> AuthorisationTypes { get; set; }

        public UnityUserViewModel(HostViewModel host) : base(host)
        {
            SaveCommand = CreateCommand(Save, (err) => Host.ShowError(err));
            CancelCommand = CreateCommand(Cancel, (err) => Host.ShowError(err));
            CloseCommand = CreateCommand(Close, (err) => Host.ShowError(err));
            EditCommand = CreateCommand(() => EditMode = true, (err) => Host.ShowError(err));
            AuthorisationTypes = Enum.GetValues(typeof(AuthorisationType)).Cast<AuthorisationType>().ToList();  
        }

        private void Save()
        {
            UnityUser.Save();
            UnityUser.Dispose();

            if (UnityUser.Data.IsWorker && !wasWorker) 
            {
                var result = MessageBox.Show("" +
                    "Aby użytkownik zaczął działać jako serwer konieczne jest ponowne zalogowanie." + Environment.NewLine + Environment.NewLine + "Chcesz to zrobić teraz?",
                    "",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                  );

                if (result == MessageBoxResult.Yes) 
                {
                    Logout();
                    Host.Window.Close();
                    return;
                }
            }

            Host.Window.Close(); 
            (Host.Window.Owner.GetHost().ControlViewModel as UnityUsersViewModel).LoadUnityUsers();
        }
        private void Cancel()
        {
            var unityUsersManager = new UnityUsersManager();
            var original = unityUsersManager.Find(UnityUser.Data);
            UnityUser.Dispose();
            UnityUser = original;
            EditMode = false;
        }
        private void Close()
        {
            UnityUser.Dispose();
            Host.Window.Close();
        }
        private void Logout()
        {
            ServicesProvider.RemoveInstance<HarmonogramWorker>()?.Dispose();
            ServicesProvider.RemoveInstance<NexoContext>()?.Dispose();
            var nexoDatabase = ServicesProvider.GetService<NexoDatabase>();
            var mvm = Host.Window.Owner.DataContext as MainViewModel;
            mvm.Window.Title = $"{nexoDatabase.Name} - {App.AppName}";
            mvm.UpdateView<LoginViewModel>();
            mvm.SidebarVisable = false;
            mvm.NavbarViewModel.Logged = false;
        }
    }
}

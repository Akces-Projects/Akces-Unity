using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.Generic;
using Akces.Wpf.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using Akces.Unity.Models;
using Akces.Unity.DataAccess.Managers;

namespace Akces.Unity.App.ViewModels
{
    internal class UnityUserViewModel : ControlViewModel
    {
        private bool editMode;
        private IUnityUser unityUser;

        public IUnityUser UnityUser { get => unityUser; set { unityUser = value; OnPropertyChanged(); } }
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
            Host.Window.Close();
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
    }
}

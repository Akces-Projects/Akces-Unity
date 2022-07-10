using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Akces.Unity.Launcher.ViewModels
{
    public class DatabaseConnectionViewModel : ControlViewModel
    {
        public static NexoDatabase selectedNexoDatabase;

        public static ObservableCollection<NexoDatabase> NexoDatabases { get; set; }
        public NexoDatabase SelectedNexoDatabase
        {
            get => selectedNexoDatabase;
            set
            {
                selectedNexoDatabase = value;
                OnPropertyChanged();
            }
        }
        public static bool RememberSettings { get; set; }
        public ICommand LaunchMainAppCommand { get; set; }
        public ICommand GoToLicenseCommand { get; set; }
        public ICommand GoToNexoFilesCommand { get; set; }
        public ICommand GoToServerConnectionCommand { get; set; }

        public DatabaseConnectionViewModel(HostViewModel host) : base(host)
        {
            LaunchMainAppCommand = CreateCommand(LaunchMainApp, (err) => Host.ShowError(err));
            GoToLicenseCommand = CreateCommand(GoToLicense, (err) => Host.ShowError(err));
            GoToNexoFilesCommand = CreateCommand(GoToNexoFiles, (err) => Host.ShowError(err));
            GoToServerConnectionCommand = CreateCommand(() => Host.UpdateView<ServerConnectionViewModel>(), (err) => Host.ShowError(err));
        }

        public void LoadDatabases()
        {
            var nexoDatabases = ServicesProvider.GetService<SqlServer>().GetNexoDatabases();
            NexoDatabases = new ObservableCollection<NexoDatabase>(nexoDatabases);
            var savedDatabase = nexoDatabases.FirstOrDefault(x => x.Name == selectedNexoDatabase.Name);

            if (selectedNexoDatabase != null && savedDatabase != null)
            {
                savedDatabase.FilesPath = selectedNexoDatabase.FilesPath;
                savedDatabase.NexoConnectionData.AutoLogin = selectedNexoDatabase.NexoConnectionData.AutoLogin;
                savedDatabase.NexoConnectionData.NexoUsername = selectedNexoDatabase.NexoConnectionData.NexoUsername;
                savedDatabase.NexoConnectionData.NexoPassword = selectedNexoDatabase.NexoConnectionData.NexoPassword;
                SelectedNexoDatabase = savedDatabase;
            }
            else
            {
                SelectedNexoDatabase = nexoDatabases.FirstOrDefault();
            }
        }
        private void GoToLicense()
        {
            ServicesProvider.RemoveInstance<NexoDatabase>();
            ServicesProvider.AddSingleton(SelectedNexoDatabase);
            Host.UpdateView<LicenseViewModel>();
        }
        private void GoToNexoFiles()
        {
            ServicesProvider.RemoveInstance<NexoDatabase>();
            ServicesProvider.AddSingleton(SelectedNexoDatabase);
            Host.UpdateView<NexoFilesViewModel>();
        }
        private void CheckNexoFilesLocation(NexoDatabase nexoDatabase)
        {
            var nexoFilesPath = nexoDatabase.FilesPath ?? nexoDatabase.GetNexoFilesPath();

            if (!string.IsNullOrEmpty(nexoFilesPath))
                nexoDatabase.FilesPath = nexoFilesPath;
            else
                Host.ShowWarning($"Nie można zlokalizaować plików nexo.{Environment.NewLine}Należy wskazać odpowiedni folder ręcznie w zakładce - Lokalizacja plików nexo");
        }
        private void LaunchMainApp()
        {
            var t = ServicesProvider.RemoveInstance<NexoDatabase>();
            ServicesProvider.AddSingleton(SelectedNexoDatabase);
            CheckNexoFilesLocation(SelectedNexoDatabase);

            if (string.IsNullOrEmpty(SelectedNexoDatabase.FilesPath))
                return;

            var licenseIsValid = ServicesProvider.GetService<NexoDatabase>().TryCheckLicense(App.ProductCode, App.LicenseKey, out License license, out string error);

            if (!licenseIsValid)
            {
                Host.ShowError(error);
                return;
            }

            File.WriteAllText("lickey", license.ToLicenseKey());
            SelectedNexoDatabase.RememberSettings = RememberSettings;
            SelectedNexoDatabase.ToFile();
            App.RunProperApplication(SelectedNexoDatabase);
            Application.Current.Shutdown();
        }
    }
}

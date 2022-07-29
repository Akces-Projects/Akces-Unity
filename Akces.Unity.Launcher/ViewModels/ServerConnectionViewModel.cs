using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http.Json;
using System;
using System.IO;
using System.Windows;
using System.Net;
using System.IO.Compression;
using Akces.Wpf.Extensions;

namespace Akces.Unity.Launcher.ViewModels
{
    public class ServerConnectionViewModel : ControlViewModel
    {
        public static string selectedServerAddress;
        public static bool winAuth;

        public static ObservableCollection<SqlServer> Servers { get; set; }
        public string SelectedServerAddress { get => selectedServerAddress; set { selectedServerAddress = value; OnPropertyChanged(); } }
        public bool WinAuth { get => winAuth; set { winAuth = value; OnPropertyChanged(); } }
        public static string DbUsername { get; set; }
        public static string DbPassword { get; set; }

        public ICommand ConnectToSqlServerCommand { get; set; }

        public ServerConnectionViewModel(HostViewModel host) : base(host)
        {
            Servers = Servers ?? new ObservableCollection<SqlServer>();
            ConnectToSqlServerCommand = CreateAsyncCommand(ConnectToSqlServerAsync, (err) => Host.ShowError(err), null, false, null);
        }

        public async Task OnStartAsync()
        {
            await UpdateAppAsync();
            if (Servers.Any()) return;

            var servers = await Task.Run(() => { return SqlServers.GetSqlServers(); });
            SelectedServerAddress = SelectedServerAddress ?? servers.FirstOrDefault()?.Server;
            RefreshCollection(Servers, servers);
        }
        private async Task ConnectToSqlServerAsync()
        {
            ServicesProvider.RemoveInstance<SqlServer>();
            var server = new SqlServer(SelectedServerAddress);
            server.SetCredentials(DbUsername, DbPassword, WinAuth);
            await Task.Run(() => server.GetNexoDatabases());
            ServicesProvider.AddSingleton(server);
            Host.UpdateView<DatabaseConnectionViewModel>().LoadDatabases();
        }

        private async Task UpdateAppAsync() 
        {
            var appUpdater = new AppUpdater();
            var isCurrentVersion = await appUpdater.IsCurrentVersionAsync();

            if (isCurrentVersion) 
                return;

            var newestVersion = await appUpdater.GetNewestVersionAsync();

            var result = MessageBox.Show(
                $"Istnieje nowa wersja aplikacji {newestVersion.tag_name}" + Environment.NewLine + Environment.NewLine +
                newestVersion.body + Environment.NewLine + Environment.NewLine +
                "Czy chcesz wykonać aktualizację teraz?",
                "Aktualizacja",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.No)
                return;

            var window = Host.CreateWindow<ExtraWindow, MainViewModel>(550, 150);
            var vm = window.GetHost().UpdateView<OperationsProgressViewModel>();
            vm.AppUpdater = appUpdater;
            window.Title = $"Aktualizacja";
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.ResizeMode = ResizeMode.NoResize;
            window.Show();
            await vm.RunOperationsAsync();
            Host.ShowInfo("Aktualizacja zakończona");
        }
    }
}

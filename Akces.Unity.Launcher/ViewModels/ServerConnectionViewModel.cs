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
            var versionFilePath = Path.Combine(App.MainAppPath, "Version.txt");

            if (!File.Exists(versionFilePath))
                File.WriteAllText(versionFilePath, "");

            var newestVersion = await GetNewestVersionAsync();
            var currentVersionNumber = File.ReadAllText(versionFilePath);

            if (newestVersion.tag_name == currentVersionNumber)
                return;

            var result = MessageBox.Show(
                $"Istnieje nowa wersja aplikacji {newestVersion.tag_name}" + Environment.NewLine + Environment.NewLine +
                newestVersion.body + Environment.NewLine + Environment.NewLine +
                "Czy chcesz wykonać aktualizację teraz?",
                "Aktualizacja",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.No)
                return;

            Host.IsBusy = true;

            using (var client = new WebClient())
            {
                var asset = newestVersion.assets.FirstOrDefault(x => x.content_type == "application/x-zip-compressed");

                if (asset?.browser_download_url == null)
                    return;

                var fileName = asset.name;
                client.DownloadFile(new Uri($"https://github.com/{App.GitHubAuthor}/{App.GitHubRepository}/releases/download/{newestVersion.tag_name}/{fileName}"), fileName);
                Directory.Delete(App.MainAppPath, true);
                ZipFile.ExtractToDirectory(fileName, App.MainAppPath);
                File.WriteAllText(versionFilePath, newestVersion.tag_name);
                File.Delete(fileName);
            }

            Host.IsBusy = false;
        }

        private async Task<GitHubVersion> GetNewestVersionAsync() 
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{App.GitHubAuthor}/{App.GitHubRepository}/releases");
                request.Headers.TryAddWithoutValidation("User-Agent", "Updater");
                var response = await client.SendAsync(request);
                var versions = await response.Content.ReadFromJsonAsync<List<GitHubVersion>>();
                var newestVersion = versions.OrderBy(x => x.published_at).LastOrDefault();
                return newestVersion;
            }
        }
    }
}

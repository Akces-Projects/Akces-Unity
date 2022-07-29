using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Akces.Unity.Launcher
{
    public delegate void OnAppUpdateStarted();
    public delegate void OnAppUpdateProgress(int progressPercentage);
    public delegate void OnAppUpdateFinished();

    public class AppUpdater 
    {
        private GitHubVersion gitHubVersion;
        private readonly string versionFilePath;
        private WebClient client;

        public OnAppUpdateStarted OnAppUpdateStarted { get; set; }
        public OnAppUpdateProgress OnAppUpdateProgress { get; set; }
        public OnAppUpdateFinished OnAppUpdateFinished { get; set; }

        public AppUpdater()
        {
            versionFilePath = Path.Combine(App.MainAppPath, "Version.txt");

            OnAppUpdateStarted = new OnAppUpdateStarted(() => { });
            OnAppUpdateProgress = new OnAppUpdateProgress((p) => { });
            OnAppUpdateFinished = new OnAppUpdateFinished(() => { });
        }

        public async Task<bool> IsCurrentVersionAsync()
        {
            var newestVersion = gitHubVersion ?? await GetNewestVersionAsync();

            if (newestVersion == null)
                return true;

            if (!File.Exists(versionFilePath))
                return false;

            return newestVersion.tag_name  == File.ReadAllText(versionFilePath);
        }
        public async Task UpdateAsync()
        {
            OnAppUpdateStarted.Invoke();

            using (client = new WebClient())
            {
                var newestVersion = gitHubVersion ?? await GetNewestVersionAsync();
                var asset = newestVersion.assets.FirstOrDefault(x => x.content_type == "application/x-zip-compressed");

                if (asset?.browser_download_url == null)
                    return;

                var fileName = asset.name;
                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                await client.DownloadFileTaskAsync($"https://github.com/{App.GitHubAuthor}/{App.GitHubRepository}/releases/download/{newestVersion.tag_name}/{fileName}", fileName);
                Directory.Delete(App.MainAppPath, true);
                ZipFile.ExtractToDirectory(fileName, App.MainAppPath);
                File.WriteAllText(versionFilePath, newestVersion.tag_name);
                File.Delete(fileName);
            }

            client = null;
            OnAppUpdateFinished.Invoke();
        }
        public void CancelAppUpdate() 
        {
            client?.CancelAsync();
        }
        public async Task<GitHubVersion> GetNewestVersionAsync()
        {
            if (gitHubVersion != null)
                return gitHubVersion;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.github.com/repos/{App.GitHubAuthor}/{App.GitHubRepository}/releases");
                request.Headers.TryAddWithoutValidation("User-Agent", "Updater");
                var response = await client.SendAsync(request);
                var versions = await response.Content.ReadFromJsonAsync<List<GitHubVersion>>();
                var newestVersion = versions.Where(x => !x.prerelease).OrderBy(x => x.published_at).LastOrDefault();
                gitHubVersion = newestVersion;
                return newestVersion;
            }
        }
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnAppUpdateProgress.Invoke(e.ProgressPercentage);
        }
    }
}

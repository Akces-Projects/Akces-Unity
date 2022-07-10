using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

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
    }
}

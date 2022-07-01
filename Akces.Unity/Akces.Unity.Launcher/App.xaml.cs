using Akces.Core.Nexo;
using Akces.Unity.Launcher.ViewModels;
using Akces.Wpf.Helpers;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Akces.Unity.Launcher
{
    public partial class App : Application
    {
        public static string LicenseKey;
        public const string ProductCode = "ABC";                          // PRODUCT CODE POWINIEN BYC INNY DLA KAZDEJ APLIKACJI
        private const string MainAppPath = "app";                         // LOKALIZACJA APLIKACJI WLASCIWEJ, KTORA MA ZOSTA URUCHOMIONA
        private const string MainAppName = "Akces.Unity.App.exe";   // NAZWA PLIKU KTORY MA ZOSTAC URUCHOMIONY


        public App()
        {
            Directory.CreateDirectory(MainAppPath);
            ViewsManager.AddViews();

            // TESTOWY KLUCZ:
            // 23443-15475-62860-694Z1-73825-XABCP-31393-0Z677-662N851

            // GENEROWANIE KLUCZA:
            var lickey = new License()
            {
                AccountingOffice = false,
                Expire = System.DateTime.Now.AddYears(50),
                IsDemo = true,
                Nip = "5542963074",
                ProductCode = ProductCode
            }.ToLicenseKey();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var settings = e.Args.Length > 0;
            LoadSettings(settings);
        }

        private void LoadSettings(bool settings)
        {
            LicenseKey = File.Exists("lickey") ? File.ReadAllText("lickey") : null;

            if (NexoDatabase.TryFromFile(out NexoDatabase nexoDatabase))
            {
                ServerConnectionViewModel.selectedServerAddress = nexoDatabase.NexoConnectionData.DbServer;
                ServerConnectionViewModel.winAuth = nexoDatabase.NexoConnectionData.WinAuth;
                ServerConnectionViewModel.DbPassword = nexoDatabase.NexoConnectionData.DbPassword;
                ServerConnectionViewModel.DbUsername = nexoDatabase.NexoConnectionData.DbUsername;
                DatabaseConnectionViewModel.selectedNexoDatabase = nexoDatabase;

                if (!settings && nexoDatabase.RememberSettings && nexoDatabase.TryCheckLicense(ProductCode, LicenseKey, out _, out _))
                {
                    RunProperApplication(nexoDatabase);
                    Current.Shutdown();
                }
            }
        }
        public static void RunProperApplication(NexoDatabase nexoDatabase)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(MainAppName, nexoDatabase.ToString())
                {
                    WindowStyle = ProcessWindowStyle.Normal,
                    WorkingDirectory = Path.GetDirectoryName($"{MainAppPath}\\{MainAppName}")
                }
            };

            process.Start();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Windows;
using Akces.Core.Nexo;
using Akces.Unity.DataAccess;
using Akces.Unity.DataAccess.Managers;
using Akces.Wpf.Helpers;
using Akces.Wpf.Extensions.Style;
using System.Globalization;
using System.Threading;

namespace Akces.Unity.App
{
    public partial class App : Application
    {
        private static NexoProduct nexoProduct = NexoProduct.Subiekt;
        public static NexoProduct NexoProduct
        {
            get => nexoProduct;
            set
            {
                if (value != nexoProduct)
                {
                    nexoProduct = value;
                    ErpName = $"{value} nexo";
                    SetStyle(value);
                }
            }
        }
        public const bool AllowChangeNexoProduct = false;

        public static string ErpName = $"{NexoProduct} nexo";
        public const string AppName = "Akces Unity";
        public const string LauncherName = "Akces.Unity.Launcher.exe";
        public const string Version = "2022.1.2";
        public const string HelpFileLocation = "{lokalizacja pliku pomocy [.doc, .pdf, .txt itd.]}";
        public const string WebsiteLink = "https://akces.pro/";
        public const string WebsiteName = "www.akces.pro";
        public const string Contact = @"AKCES S.C                                                              
                                                                                   
ADRES :                            ul. Lotników 7, 87-100 Toruń        
E-MAIL :                            biuro@akces.pro            
TELEFON :                        +48 56 660 23 45 lub +48 56 652 70 65
GODZINY OTWARCIA :    Poniedziałek – Piątek 8:00 – 16:00";


        public App()
        {
            ViewsManager.AddViews();
            SetStyle(NexoProduct);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // DLA TESTOW:
            NexoDatabase.TryFromFile(out NexoDatabase nexoDatabase, "..\\localdata");

            // W WERSJI OSTATECZNEJ:
            //var nexoDatabase = ServicesProvider.AddSingleton(NexoDatabase.FromString(e.Args[0]));

            ServicesProvider.AddSingleton(nexoDatabase);
            UnityConnection.ConnectionString = nexoDatabase.NexoConnectionData.GetConnectionString(useInitialCatalog: false) + "Database=Unity.DataCenter;";
            UnityConnection.EnsureCreated();
            CheckLicense(nexoDatabase);
            InitUnityUsers(nexoDatabase);
        }

        private void CheckLicense(NexoDatabase nexoDatabase) 
        {
            var lickey = File.ReadAllText("..\\lickey");
            var licenseIsValid = nexoDatabase.TryCheckLicense("UNT", lickey, out _, out _);
            if (!licenseIsValid)
                Current.Shutdown();
        }
        private void InitUnityUsers(NexoDatabase nexoDatabase) 
        {
            var nexoUsers = nexoDatabase.GetNexoUsers();
            var unityUsersManager = new UnityUsersManager();
            var unityUsers = unityUsersManager.Get();

            foreach (var nexoUser in nexoUsers)
            {
                if (unityUsers.Any(x => x.Login == nexoUser.Login))
                    continue;

                using (var userBO = unityUsersManager.Create())
                {
                    userBO.Data.Login = nexoUser.Login;
                    userBO.Data.Name = nexoUser.Name;
                    userBO.Save();
                }
            }
        }
        public static void SetStyle(NexoProduct nexoProduct)
        {
            var akcesStyleTheme = (AkcesStyleTheme)typeof(AkcesStyleTheme)
                .GetFields()
                .First(x => x.Name == Enum.GetName(typeof(NexoProduct), nexoProduct))
                .GetValue(null);

            AkcesStyle.SetStyle(akcesStyleTheme);
            var mw = Current.MainWindow as MainWindow;
            mw?.RebuildAsync();

            // Global DateTime format
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}

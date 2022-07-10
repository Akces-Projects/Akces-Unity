using Akces.Core.Nexo;
using Akces.Wpf.Helpers;
using Akces.Wpf.Models;
using System.Windows.Input;

namespace Akces.Unity.Launcher.ViewModels
{
    public class LicenseViewModel : ControlViewModel
    {
        public string LicenseKey { get => App.LicenseKey; set { App.LicenseKey = value; } }
        public ICommand CheckLicenseCommand { get; set; }
        public ICommand GoToDatabaseConnectionCommand { get; set; }

        public LicenseViewModel(HostViewModel host) : base(host)
        {
            CheckLicenseCommand = CreateCommand(CheckLicense, (err) => Host.ShowError(err));
            GoToDatabaseConnectionCommand = CreateCommand(() => Host.UpdateView<DatabaseConnectionViewModel>(), (err) => Host.ShowError(err));
        }

        public void CheckLicense()
        {
            var licenseIsValid = ServicesProvider.GetService<NexoDatabase>().TryCheckLicense(App.ProductCode, LicenseKey, out License license, out string error);

            if (licenseIsValid)
                Host.ShowInfo(license.ToString());
            else
                Host.ShowError(error);
        }
    }
}

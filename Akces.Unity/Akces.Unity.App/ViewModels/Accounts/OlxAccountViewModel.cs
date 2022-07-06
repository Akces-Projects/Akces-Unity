using Akces.Wpf.Models;
using Akces.Unity.DataAccess.Managers.BusinessObjects;
using Akces.Unity.Models.SaleChannels.Olx;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace Akces.Unity.App.ViewModels
{
    internal class OlxAccountViewModel : AccountViewModel<OlxAccount>
    {
        private IAccount<OlxAccount> account;
        public override IAccount<OlxAccount> Account { get => account; set { account = value; OnPropertyChanged(); } }
        public OlxAccountViewModel(HostViewModel host) : base(host) { }


        protected override async Task AuthenticateAsync()
        {
            var result = MessageBox.Show(
                "Zaloguj się na konto Olx zanim rozpoczniesz autoryzację. " + Environment.NewLine + Environment.NewLine +
                "OK - rozpocznij autoryzację, " + Environment.NewLine +
                "Anuluj - przerwij",
                "Autoryzacja", MessageBoxButton.OKCancel, MessageBoxImage.Information); ;

            if (result != MessageBoxResult.OK)
                return;

            await base.AuthenticateAsync();
        }
    }
}

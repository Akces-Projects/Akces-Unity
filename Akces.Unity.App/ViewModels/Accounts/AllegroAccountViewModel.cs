using Akces.Wpf.Models;
using Akces.Unity.Models.SaleChannels;
using Akces.Unity.DataAccess.Managers.BusinessObjects;

namespace Akces.Unity.App.ViewModels
{
    internal class AllegroAccountViewModel : AccountViewModel<AllegroAccount>
    {
        private IAccount<AllegroAccount> account;
        public override IAccount<AllegroAccount> Account { get => account; set { account = value; OnPropertyChanged(); } }
        public AllegroAccountViewModel(HostViewModel host) : base(host) { }
    }
}
